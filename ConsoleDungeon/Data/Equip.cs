using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// 열겨형 장비 타입 = 장신구 / 무기구 / 방어구 / 아이템
public enum EquipType
{
    Accessory = 0,
    Weapon = 1,
    Armor = 2,
    Item = 3,
}

// 장비 클래스
public class Equip
{
    public string Name { get; private set; }
    public EquipType Type { get; private set; }
    public string Effect { get; private set; }
    public string Sub { get; private set; }
    public char State { get; private set; }
    public int Price { get; private set; }
    public int Index { get; private set; }          // 효과 적용을 위한 장비 시리얼 넘버
    public bool IsEquipped { get; private set; }    // 장착 확인 변수

    public Equip(string _name, EquipType _type, string _effect, string _sub, int _price, int _index)
    {
        Name = _name;
        Type = _type;
        Effect = _effect;
        Sub = _sub;
        Price = _price;
        State = ' ';
        Index = _index;
        IsEquipped = false;
    }

    // 장비 장착 / 해제
    public void Switch(Player player)
    {
        // 장비 장착
        if (!IsEquipped)
        {
            // 이미 해당 타입의 장비가 장착되어 있을 때 해제 먼저
            if (player.OnEquip[(int)Type] != null)
            {
                player.OnEquip[(int)Type].State = ' ';
                player.OnEquip[(int)Type].IsEquipped = false;
                player.EquipEffect(player.OnEquip[(int)Type]);
                player.OnEquip[(int)Type] = null;
            }

            // 해제 된 이후 현재 장비 장착
            player.OnEquip[(int)Type] = this;
            State = 'E';
            player.EquipEffect(this);
            IsEquipped = true;
        }
        // 장비 해제
        else
        {
            player.OnEquip[(int)Type] = null;
            State = ' ';
            player.EquipEffect(this);
            IsEquipped = false;
        }
    }
}

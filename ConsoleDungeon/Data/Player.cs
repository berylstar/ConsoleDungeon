using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class Player : ICharacter
{
    public string Name { get; private set; }
    public string Job { get; private set; }
    public int Level { get; private set; }
    public int AP { get; private set; }
    public int DP { get; private set; }
    public int HP { get; private set; }
    public int Speed { get; private set; }
    public int Gold { get; set; }
    public List<Equip> Inventory { get; private set; }      // 가지고 있는 장비
    public List<Equip> OnEquip { get; private set; }        // 장착되어 있는 장비

    private int exp = 0;        // 레벨 업을 위한 경험치

    public bool DefenseStance { get; set; }     // 방어 태세
    public int Focus { get; set; }              // 집중 카운트

    public Player(string _name, string _job, int _ap, int _dp, int _hp, int _speed)
    {
        Name = _name;
        Job = _job;
        Level = 1;
        AP = _ap;
        DP = _dp;
        HP = _hp;
        Speed = _speed;
        Gold = 1000;
        Inventory = new List<Equip>()       // 기본 지급 장비
        {
            new Equip("무쇠 갑옷", EquipType.Armor, "방어력 +1", "무쇠로 만들어져 튼튼한 갑옷입니다.", 0, 1),
            new Equip("낡은 검", EquipType.Weapon, "공격력 +2", "쉽게 볼 수 있는 낡은 검입니다.", 0, 2),
        };
        OnEquip = new List<Equip>() { null, null, null, null };
        DefenseStance = false;
        Focus = 0;
    }

    // 인벤토리의 장비 이름 반환, 없으면 ""
    public string OnEquipName(int idx)
    {
        if (OnEquip[idx] == null)
            return " ";
        else
            return OnEquip[idx].Name;
    }

    // 장비 획득
    public void GetEquip(Equip _equip)
    {
        Inventory.Add(_equip);
    }

    // 장비 판매
    public void SellEquip(Equip _equip)
    {
        // 장착되어 있으면 효과 해제 이후 판매
        if (_equip.IsEquipped)
            EquipEffect(_equip);

        Inventory.Remove(_equip);
    }

    // 장비 효과 적용 / 해제
    public void EquipEffect(Equip equip)
    {
        int pm = equip.IsEquipped ? -1 : 1;

        if (equip.Index == 1) { DP += 1 * pm; }
        else if (equip.Index == 2) { AP += 2 * pm; }
        else if (equip.Index == 3) { DP += 7 * pm; }
        else if (equip.Index == 4) { AP -= 2 * pm; DP += 15 * pm; }
        else if (equip.Index == 5) { DP += 5 * pm; HP += 10 * pm; }
        else if (equip.Index == 6) { DP += 7 * pm; Speed -= 10 * pm; }
        else if (equip.Index == 7) { DP += 3 * pm; }
        else if (equip.Index == 8) { AP += 5 * pm; Speed -= 10 * pm; }
        else if (equip.Index == 9) { AP += 2 * pm; Speed += 10 * pm; }
        else if (equip.Index == 10) { AP += 7 * pm; HP -= 15 * pm; }
        else if (equip.Index == 11) { AP += 10 * pm; }
        else if (equip.Index == 12) { Speed += 20 * pm; }
        else if (equip.Index == 13) { DP += 5 * pm; }
        else if (equip.Index == 14) { HP += 5 * pm; }
        else if (equip.Index == 15) { AP += 10 * pm; DP -= 20 * pm; }
        else if (equip.Index == 16) { Speed += 10 * pm; }
        else if (equip.Index == 17) { AP -= 2 * pm; HP += 20 * pm; }
        else if (equip.Index == 18) { DP += 10 * pm; HP -= 10 * pm; }
        else if (equip.Index == 19) { AP += 3 * pm; }
        else if (equip.Index == 20) { AP += 1 * pm; Speed += 10 * pm; }
        else if (equip.Index == 21) { HP += 30 * pm; }
        else if (equip.Index == 22) { AP += 5 * pm; Speed -= 10 * pm; }
    }

    // 경험치 획득 + 레벨 업 확인
    public void GetExperience(int val)
    {
        exp += val;

        while (Level <= exp)
        {
            exp -= Level;

            Utils.Talk("\n ※ 레벨 업 ! ※");
            Utils.Talk($" ※공격력 + {(int)Math.Ceiling(Level / 3f)}, 체력 + 10 ※");

            AP += (int)Math.Ceiling(Level / 3f);
            HP += 10;

            Level += 1;
        }
    }

    // 피해 입었을 때
    public void GetDamaged(int damage)
    {
        HP -= damage;
    }

    // 방어 여부 판단
    public bool IsDefense()
    {
        if (Utils.random.Next(0, 101) <= DP)
            return true;
        else if (DefenseStance)
            return true;
        else
            return false;
    }
}
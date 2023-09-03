using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 상점 클래스 - 장비 사고 팔기
public class Shop
{
    public List<Equip> AllEquips { get; private set; }
    public List<Equip> ShopList { get; private set; }

    private readonly Player player;

    public Shop(Player _player)
    {
        player = _player;

        // 전체 장비 초기화
        AllEquips = new List<Equip>()
        {
            new Equip("수련자의 도복", EquipType.Armor, "방어력 +7", "수련 집중에 효과적입니다.", 2500, 3),
            new Equip("청동 갑옷", EquipType.Armor, "방어력 +15, 공격력 -2", "확실히 무거운 갑옷입니다.", 1000, 4),
            new Equip("호랑이 가죽", EquipType.Armor, "방어력 +5, 체력 +10", "호랑이는 죽어서 가죽을 남기고", 1500, 5),
            new Equip("검투사의 보호대", EquipType.Armor, "방어력 +7, 스피드 -10", "검투사 전용 보호구입니다.", 1800, 6),
            new Equip("천 갑옷", EquipType.Armor, "방어력 +3", "가성비 좋은 갑옷입니다.", 500, 7),

            new Equip("커다란 대검", EquipType.Weapon, "공격력 +5, 스피드 -10", "엄청나게 크고 무겁습니다.", 1000, 8),
            new Equip("대나무 창", EquipType.Weapon, "공격력 +2, 스피드 +10", "가볍고 날카롭습니다.", 1500, 9),
            new Equip("양날의 검", EquipType.Weapon, "공격력 +7, 체력 -15", "말 그대로 \"양날의 검\"", 800, 10),
            new Equip("검은 검", EquipType.Weapon, "공격력 +10", "sword = sword", 3000, 11),
            new Equip("단검", EquipType.Weapon, "스피드 +20", "작지만 누구보다 빠르다.", 1200, 12),

            new Equip("빛나는 왕관", EquipType.Accessory, "방어력 +5", "아직은 반짝반짝합니다.", 800, 13),
            new Equip("낡은 투구", EquipType.Accessory, "체력 +5", "누군가가 사용했던 것 같습니다.", 0, 14),
            new Equip("붉은 목걸이", EquipType.Accessory, "공격력 +10, 방어력 -20", "쓴 사람은 저주를 받습니다.", 1200, 15),
            new Equip("낡은 후드", EquipType.Accessory, "스피드 +10", "얼굴을 가리는데 효과적입니다.", 500, 16),
            new Equip("검은 반지", EquipType.Accessory, "체력 +20, 공격력 -2", "쇠약해지지만, 살아남을 겁니다.", 800, 17),

            new Equip("부적", EquipType.Item, "방어력 +10, 체력 -10", "용한 부적입니다.", 1500, 18),
            new Equip("맹독병", EquipType.Item, "공격력 +3", "무기에 발라보세요.", 1200, 19),
            new Equip("보급형 아이템", EquipType.Item, "공격력 +1, 스피드 +5", "초보 모험가를 위한 아이템.", 0, 20),
            new Equip("비상약", EquipType.Item, "체력 +30", "비상시를 대비합니다.", 1500, 21),
            new Equip("모래 주머니", EquipType.Item, "공격력 +5, 스피드 -10", "모래주머니 수련법", 800, 22),
        };

        ShopList = new List<Equip>();

        SetShop();
    }

    // 상점에서는 무작위의 5개의 상품만 판매
    public void SetShop()
    {
        foreach (Equip equip in ShopList)
        {
            AllEquips.Add(equip);
        }

        ShopList.Clear();

        for (int i = 0; i < 5; i++)
        {
            int idx = Utils.random.Next(0, AllEquips.Count);

            ShopList.Add(AllEquips[idx]);
            AllEquips.RemoveAt(idx);
        }
    }

    // 상점에서 장비 구입
    public void Buy(int index)
    {
        player.Gold -= ShopList[index].Price;
        player.GetEquip(ShopList[index]);
        ShopList[index] = null;
    }

    // 상점에서 장비 판매
    public void Sell(int index)
    {
        Equip equip = player.Inventory[index];
        player.SellEquip(equip);
        player.Gold += equip.Price / 2;
    }
}
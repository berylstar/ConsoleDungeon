using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 캐릭터 인터페이스 : 플레이어, 몬스터들의 기본 스탯 = 이름 / 레벨 / 공격력 / 방어력 / 체력 / 스피드
public interface ICharacter
{
    string Name { get; }
    int Level { get; }
    int AP { get; }
    int DP { get; }
    int HP { get; }
    int Speed { get; }

    bool DefenseStance { get; set; }
    int Focus { get; set; }

    void GetDamaged(int damage);
    bool IsDefense();
}
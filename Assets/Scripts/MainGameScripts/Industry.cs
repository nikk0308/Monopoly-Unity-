using System.Collections.Generic;
using UnityEngine;

public class Industry
{
    public List<Position> enterprisesIndexes;
    public string industryName;
    public Color color;

    public Industry(List<Position> enterprisesIndexes, string industryName, Color color) {
        this.color = color;
        this.enterprisesIndexes = enterprisesIndexes;
        this.industryName = industryName;
    }

    public List<Enterprise> GetEnterprisesInIndustry(Field field) {
        List<Enterprise> ans = new List<Enterprise>();

        foreach (var pos in enterprisesIndexes) {
            ans.Add(field.fieldArrays[pos.arrayIndex][pos.cellIndex] as Enterprise);
        }

        return ans;
    }
}

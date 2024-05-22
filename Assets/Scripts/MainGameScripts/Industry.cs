using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Industry : MonoBehaviour
{
    public List<Position> enterprisesIndexes;
    public string industryName;
    public ConsoleColor color;

    public Industry(List<Position> enterprisesIndexes, string industryName, ConsoleColor color) {
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

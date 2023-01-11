using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using NetUtilityLib;
using System.Text.RegularExpressions;

public class KeyCaculate : MonoBehaviour
{
    private List < string > signs = new List < string > ();
    private List < float > nums = new List < float > ();

    private void Awake() {
    }
    private string pathExcel = @"C:\Users\开发11\Desktop\configs\SS.xlsx";
    private void Start() {
        string d="dd";
        Type typeName=d.GetType();
    }

    //提取浮点数
    public double GetFloatNum(string s) {
        signs.Clear();
        nums.Clear();
        MatchCollection vMatchs = Regex.Matches(s, @"(\D+\.\D+|(\D+))");
        int[] vInts = new int[vMatchs.Count];
        for (int i = 0; i < vMatchs.Count; i++) {
            if (vMatchs[i].Value != ".") {
                signs.Add(vMatchs[i].Value);
                Debug.Log(vMatchs[i].Value);
            }

        }

        MatchCollection vMatchs1 = Regex.Matches(s, @"(\d+\.\d+|(\d+))");
        int[] vInts1 = new int[vMatchs1.Count];
        for (int i = 0; i < vMatchs1.Count; i++) {
            nums.Add(Convert.ToSingle(vMatchs1[i].Value));
            Debug.Log(vMatchs1[i].Value);
        }
        return 0;//(GetValue());
    }
    //设置优先级-()>
    private double GetValue() {
        double temp = 0;
        for (int i = 0; i < signs.Count; i++) {
            var sign = signs[i];
            switch (sign) {
                case "+": {
                    if (i == 0) {
                        temp = nums[i] + nums[i + 1];
                    } else {
                        temp += nums[i + 1];
                    }
                }
                break;
            case "-":
                if (i == 0) {
                    temp = nums[i] - nums[i + 1];
                } else {
                    temp -= nums[i + 1];
                }
                break;
            case "*":
                if (i == 0) {
                    temp = nums[i] * nums[i + 1];
                } else {
                    temp *= nums[i + 1];
                }
                break;
            case "/":
                if (i == 0) {
                    temp = nums[i] / nums[i + 1];
                } else {
                    temp /= nums[i + 1];
                }
                break;
            case "^":
                if (i == 0) {
                    temp = Math.Pow(Convert.ToDouble( nums[i]),Convert.ToDouble(nums[i+1]));
                } else {
                    temp = Math.Pow(temp,Convert.ToDouble(nums[i+1]));
                }
                break;
            }
        }
        return temp;
    }
}

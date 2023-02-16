using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IChooseItem {
    bool isUse { get; set; }
    bool isChoose { get; set; }
    bool isCursor { get; set; }
    void SetUse();
    void SetCursor();
    void SetChoose();
    void SetDefault();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILineable
{
    bool isInLine { get; }
    int PositionInLine { get; }
    Line CurrentLine { get; }

    void MoveInLine();
    void JoinLine(Line line);
    void LeaveLine();
}

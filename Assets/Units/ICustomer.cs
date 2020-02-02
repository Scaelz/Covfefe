
using System.Collections.Generic;
using UnityEngine;

public interface ICustomer
{
    //List<ICacheBox> ShopList();
    void Leave();
    void GoShoping();
    void GetInLine();
    void Idle();
    void MoveInLine();
}

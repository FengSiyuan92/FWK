using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public enum MsgType
    {
        none = 0,
/* frame message*/

        openUI,
        closeUI,

/* logic message*/


   // player behaviour
   action_move,
   action_fire,
   action_skill,

        maxValue
    }

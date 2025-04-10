using UnityEngine;
using static FlatCalendar2;

public class CalendarInit : MonoBehaviour
{
    //Ref to the Calendar prefab
    public FlatCalendar2 calendar;

    void Start()
    {
        //Set the event callback 
        calendar.setCallback_OnTriggerEvent(Notify);

        //init the calendar (important)
        calendar.initFlatCalendar();
    }

    //Method called when an event occurs
    public void Notify(EventObj evnt)
    {
        evnt.print();
    }
}

/**
 * Flat Calendar
 * 
 * This class manage logic of Flat Calendar
 *
 * @version 2.0
 * @authors  Gerardo Ritacco, Aldo Meo
 * @email   gerardo.ritacco@3dresearch.it, aldo.meo@3dresearch.it
 * @company 3dresearchsrl
 * @website http://www.3dresearch.it/
 * 
 * Copyright © 2016 by 3dresearchsrl
 *
 * All rights reserved. No part of this publication may be reproduced, distributed, 
 * or transmitted in any form or by any means, including photocopying, recording, or 
 * other electronic or mechanical methods, without the prior written permission of the 
 * publisher, except in the case of brief quotations embodied in critical reviews and 
 * certain other noncommercial uses permitted by copyright law. For permission requests, 
 * write to the publisher, addressed “Attention: Permissions Coordinator,” at the address below.
 */

#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using System.Globalization;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using static FlatCalendar2.EventExportList;
using UnityEngine.UI.Extensions;

using Models;
using DataRepositories;

public class FlatCalendar2 : MonoBehaviour
{

	/**
	 * Max day slots (DO NOT CHANGE THIS VALUE)
	 */
	public static readonly int max_day_slots = 38;

	/**
	 * Current UI Style
	 */
	public int current_UiStyle;

    /**
	 * Start Options
	 */
    public bool eventNotification;
    public bool startWeekFromSunday;
    public int current_UiLanguage;
    public bool initCalendarFromFile;
    public string filePath;
    private int currentDaySlot;

    List<String> MachineNames;

    /**
    * Gameobjects Fields 
     */
    public GameObject[] daysOfWeek_events_label;
    public GameObject[] sliderDaysButtons;
    public GameObject[] sliderMonthsButtons;
    public GameObject[] sliderYearsButtons;

    public GameObject btn_SwitchModality;
    public GameObject slider;

    //0 = SliderDaysButtons ScrollRect
    //1 = SliderMonthButtons ScrollRect
    //2 = SliderYearButtons ScrollRect
    public ScrollRect[] scrollRects;
    public GameObject grid;

    public GameObject btn_nextYear;
    public GameObject btn_prevYear;
    public GameObject btn_nextMonth;
    public GameObject btn_prevMonth;
    public GameObject btn_calendar;
    public GameObject label_year;
    public GameObject label_month;
    public GameObject label_dayOfWeek;
    public GameObject label_dayNumber;

    public GameObject event_Visualizer;
    public GameObject event_Visualizer_backButton;
    
    //0 = slider Button
    //1 = grid Button
    //2 = back Button
    //3 = set alarm
    public GameObject[] btn_AddEvent;
    bool currentAlarmValue = true;
    public GameObject addEvent_Visualizer;
    public GameObject addEvent_Detail_Visualizer;
    public GameObject eventObj;
    private EventObj modifingEvent;
    public GameObject eventsScrollView;
    public GameObject eventObjcContent;
    public GameObject confirmDelete;
    public GameObject deleteEventBox;

    //0 = hours slider
    //1 = minutes slider
    public GameObject[] field_TimeSlider;
    private int selected_hours = -1;
    private int selected_minutes = -1;

    //0 = NewEvent
    //1 = Event time
    //2 = Event title
    //3 = Event description
    //4 = Todays Events
    //5 = Save Button
    //6 = Today's Events
    public GameObject[] add_event_labels;
    public GameObject field_Name;
    public GameObject label_Date;
    public GameObject field_timeError;
    public GameObject field_Description;
    public GameObject btn_Details;
    public GameObject dropdown_Machine;

    //0 = NewEvent_Details
    //1 = Event time
    //2 = Event title
    //3 = Event description
    //4 = Todays Events
    //5 = Save Button
    //6 = Today's Events
    public GameObject[] add_event_detail_labels;
    public GameObject label_Date_Detail;
    public GameObject field_User;
    public GameObject field_Machine;
    public GameObject btn_Confirm;


    /**
	 * Export Event class Object
	 */
    public class EventExportList
    {
        public List<EventExport> calendar { get; set; }

        public class EventExport
        {
            public bool alarm { get; set; }
            public DateTime day { get; set; }
            public string name { get; set; }
            public string description { get; set; }
        }
    }

    /**
	 * Event class Object
	 */
    public class EventObj
	{
        public bool alarm = true;
		public string name;
		public string description;
        public TimeSpan ? time;

		public EventObj(string _name, string _description, TimeSpan _time)
		{
			name 		= _name;
			description = _description;
            time = _time;
		}

        public EventObj(string _name, string _description)
        {
            name = _name;
            description = _description;
            time = null;
        }

        public void print()
		{
			Debug.Log("Name Event: " + name + " Description Event: " + description);
		}
	}

	/**
	 * Time Structure Object 
	 */
	public struct TimeObj
	{
		public int    year;
		public int    month;
		public int    day;
		public int    totalDays;
		public string dayOfWeek;
		public int    dayOffset;

		public TimeObj(int _year,int _month,int _day, int _totalDays, string _dayOfWeek, int _dayOffset)
		{
			year      = _year;
			month     = _month;
			day       = _day;
			totalDays = _totalDays;
			dayOffset = _dayOffset;
			dayOfWeek = _dayOfWeek;
		}

		public void print()
		{
			Debug.Log("Year:"+year+" Month:"+month+" Day:"+day+" Day of Week:"+dayOfWeek);
		}
	}

	/**
	 * Current Time 
	 */
	public TimeObj currentTime;

    /**
	 * Notifyed Events 
	 */
    public static List<EventObj> notifyedEvents;

	/**
	 * Event List 
	 */
	public static Dictionary<int,Dictionary<int,Dictionary<int,List<EventObj>>>> events_list; // <Year,<Month,<Day,Number of Events>>>

	/**
	 * Delegate Callbacks 
	 */
	public delegate void Delegate_OnDaySelected(TimeObj time);
	public delegate void Delegate_OnEventSelected(TimeObj time, List<EventObj> evs);
	public delegate void Delegate_OnMonthChanged(TimeObj time);
	public delegate void Delegate_OnNowDay(TimeObj time);
    public delegate void Delegate_OnTriggerEvent(EventObj evnt);
    public Delegate_OnDaySelected   delegate_ondayselected;
	public Delegate_OnEventSelected delegate_oneventselected;
	public Delegate_OnMonthChanged  delegate_onmonthchanged;
	public Delegate_OnNowDay		delegate_onnowday;
    public Delegate_OnTriggerEvent  delegate_ontriggerevent;


    // Use this for initialization
    public void initFlatCalendar()
	{	
        // Enable components to be find by the StyleClass 
        grid.SetActive(true);
        event_Visualizer.SetActive(true);
        slider.SetActive(true);
        addEvent_Visualizer.SetActive(true);
        addEvent_Detail_Visualizer.SetActive(true);

        // Add Event Listeners
        addEventsListener();

        // Apply UI Color style
        FlatCalendarStyle2.changeUIStyle(current_UiStyle);

        // Disable components
        event_Visualizer.SetActive(false);
        slider.SetActive(false);
        addEvent_Visualizer.SetActive(false);
        addEvent_Detail_Visualizer.SetActive(false);

        // Set current time
        setCurrentTime();

        // Initialize event list
        events_list = new Dictionary<int, Dictionary<int, Dictionary<int,List<EventObj>>>>();

        // Initialize notifyed events list
        notifyedEvents = new List<EventObj>();

        //TODO Cargue de listar para combos (dropdowlist)        
        #region CARGUE COMBOS
        IMachineReposiory ds = new MachineRepository();
        List<Machine> MachineList = ds.GetAll();
        MachineNames = MachineList.Select(m => m.Name).ToList();
        ChargeDropdown(dropdown_Machine,MachineNames);

        IMaintananceActivity dsMA = new MaintananceActivityRepository();
        List<MaintananceActivity> MaintananceActivities = dsMA.GetByMachine(1);
        
        
        #endregion

		// Update Calendar with Current Data
		updateCalendar(currentTime.month,currentTime.year);

        // start thread for notification process
        if (eventNotification && delegate_ontriggerevent != null)
            StartCoroutine(CheckEventNotification());

        // Set path and try import to events data
        if (initCalendarFromFile)
        {
            if (filePath == "")
                filePath = Application.persistentDataPath + "/events.json";
            importFlatCalendarEvents(filePath);
        }
        else filePath = "";

        // Show events
        populateEventVisualizer();

    }    

    //Carga los dropdownlist
    private void ChargeDropdown(GameObject _dropdown, List<string> MachineNames) 
    {
        Dropdown a = _dropdown.transform.GetComponent<Dropdown>();

        a.options.Clear();

        a.options.Add(new Dropdown.OptionData() {text = "Seleccione..."} );

        foreach(string name in MachineNames)
        {
            a.options.Add(new Dropdown.OptionData() {text = name} );
        }

        a.value = 0;
    }


    // Use this for storing events as json file
    // The path parameter is the saving location with the file name.json
    public void exportFlatCalendarEvents(string path)
    {
        List<EventExport> calendar  = new List<EventExport>();
        foreach (var year_items in events_list)
        {
            int year = year_items.Key;
            foreach (var month_items in events_list[year])
            {
                int month = month_items.Key;
                foreach (var day_items in events_list[year][month])
                {
                    int day = day_items.Key;
                    List<EventObj> eventsObj = events_list[year][month][day];
                    foreach (EventObj eventObj in eventsObj)
                    {
                        EventExport current = new EventExport();
                        TimeSpan time;

                        if (eventObj.time != null)
                        {
                            time = (TimeSpan) eventObj.time;
                            current.day = new DateTime(year, month, day, time.Hours, time.Minutes, 00);
                        }
                        else
                            current.day = new DateTime(year, month, day, 00, 00, 01);

                        current.name = eventObj.name;
                        current.alarm = eventObj.alarm;
                        current.description = eventObj.description;
                        calendar.Add(current);
                    }
                }
            }
        }
        calendar = calendar.OrderBy(item => item.day).ToList();
        string json = JsonConvert.SerializeObject(calendar);
        try
        {
            File.WriteAllText(path, json);
            Debug.Log("File Saved");
        }
        catch (Exception)
        {
            Debug.Log("File invalid path");
        }
    }

    // Use this for import your events as json file
    // The path parameter is the current json file location with the file name
    public void importFlatCalendarEvents(string path)
    {
        string json;
        List<EventExport> calendar;
        if (File.Exists(path))
            json = File.ReadAllText(path);
        else
        {
            if(filePath != Application.persistentDataPath + "/events.json")
                Debug.Log("File invalid path");
            return;
        }
        try
        {
            calendar = JsonConvert.DeserializeObject<List<EventExport>>(json);
        }
        catch (Exception)
        {
            Debug.Log("File invalid format");
            return;
        }

        foreach (EventExport evnt in calendar)
        {
            TimeSpan invalid = new TimeSpan(00, 00, 01);
            TimeSpan time = new TimeSpan(evnt.day.Hour, evnt.day.Minute, evnt.day.Second);
            EventObj eventObj;
            if(invalid.Equals(time))
                eventObj = new EventObj(evnt.name, evnt.description);
            else 
                eventObj = new EventObj(evnt.name, evnt.description, time);

            eventObj.alarm = evnt.alarm;
            addEvent(evnt.day.Year, evnt.day.Month, evnt.day.Day, eventObj);
        }
    }

	void Start () { }

	void Update()    {
        
    }

    IEnumerator CheckEventNotification()
    {
        DateTime now = DateTime.Now;
        if (checkEventExist(now.Year, now.Month, now.Day))
        {
            List<EventObj> todays_events = events_list[now.Year][now.Month][now.Day];
            foreach (EventObj currentEvent in todays_events)
            {
                TimeSpan now_time = new TimeSpan(now.Hour, now.Minute, now.Second);
                if (now_time > currentEvent.time && currentEvent.alarm)
                {
                    if (!notifyedEvents.Contains(currentEvent))
                    {
                        delegate_ontriggerevent(currentEvent);
                        notifyedEvents.Add(currentEvent);
                    }
                }
            }
        }
        yield return new WaitForSeconds(3);
        if(eventNotification)
            StartCoroutine(CheckEventNotification());
    }

    // Update Calendar
    public void updateCalendar(int month_number, int year)
	{
		// Populate day slots
		populateAllSlot(month_number,year);

		// Update Year and Month Label
		label_year.GetComponent<Text>().text      = "" + currentTime.year;
		label_month.GetComponent<Text>().text     = getMonthStringFromNumber(currentTime.month);
        label_year.GetComponent<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        label_month.GetComponent<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        markSelectionDay(currentTime.day);
        onSliderUpdateCaledar();
    }
    public void onSliderUpdateCaledar()
    {
        //set Days
        foreach (GameObject button in sliderDaysButtons)
        {
            button.SetActive(false);
            button.GetComponentInChildren<Image>().enabled = false;
        }

        for (int i = 0; i < currentTime.totalDays; i++)
        {
            sliderDaysButtons[i].SetActive(true);
            Text[] day = sliderDaysButtons[i].GetComponentsInChildren<Text>();
            day[1].text = getDayOfWeek(currentTime.year, currentTime.month, i + 1);

            if (checkEventExist(currentTime.year, currentTime.month, i + 1))
            {
                sliderDaysButtons[i].GetComponentInChildren<Image>().enabled = true;
                sliderDaysButtons[i].GetComponentInChildren<Image>().color = FlatCalendarStyle2.color_bubbleEvent;
            }

            foreach (Text t in day)
            {
                if (currentTime.day == i + 1)
                {
                    t.color = FlatCalendarStyle2.color_bubbleSelectionMarker;
                    //scrollRects[0].content.localPosition = scrollRects[0].GetSnapToPositionToBringChildIntoView(sliderDaysButtons[i].GetComponent<RectTransform>());
                    scrollRects[0].GetComponent<HorizontalScrollSnap>().GoToScreen(i);
                }
                else
                    t.color = FlatCalendarStyle2.color_dayTextNormalGrid;
            }
        }

        //set Month
        for(int i=0; i< 12; i++)
        {
            sliderMonthsButtons[i].GetComponentInChildren<Text>().text = getMonthStringFromNumber(i+1);
            if(i+1 == currentTime.month)
            {
                sliderMonthsButtons[i].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
                //scrollRects[1].content.localPosition = scrollRects[1].GetSnapToPositionToBringChildIntoView(sliderMonthsButtons[i].GetComponent<RectTransform>());
                scrollRects[1].GetComponent<HorizontalScrollSnap>().GoToScreen(i);
            }

            else sliderMonthsButtons[i].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_dayTextNormalGrid;
        }

        //set Year
        for (int i = 0; i < 21; i++)
        {
            sliderYearsButtons[i].GetComponentInChildren<Text>().text = (currentTime.year-10 + i).ToString();
            if(i == 10)
            {
                sliderYearsButtons[i].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
                //scrollRects[2].content.localPosition = scrollRects[2].GetSnapToPositionToBringChildIntoView(sliderYearsButtons[i].GetComponent<RectTransform>());
                //ScrollRectExtensions.SnapTo(sliderYearsButtons[10].GetComponent<RectTransform>(), scrollRects[2], scrollRects[2].content);
                scrollRects[2].GetComponent<HorizontalScrollSnap>().GoToScreen(i);
            }
            else
                sliderYearsButtons[i].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_dayTextNormalGrid;

        }

        //set Events       
        populateEventVisualizer();
    }    

    public void refreshCalendar()
	{
		populateAllSlot(currentTime.month,currentTime.year);
        markSelectionDay(currentTime.day);
    }

    /* Get Month String from Montth Number 
	 * 
	 * Example: Genuary <====> 1
	 */
    string getMonthStringFromNumber(int month_number)
	{
		string month = "";
        if(current_UiLanguage == 0)
        {
            if (month_number == 1) month = "January";
            if (month_number == 2) month = "February";
            if (month_number == 3) month = "March";
            if (month_number == 4) month = "April";
            if (month_number == 5) month = "May";
            if (month_number == 6) month = "June";
            if (month_number == 7) month = "July";
            if (month_number == 8) month = "August";
            if (month_number == 9) month = "September";
            if (month_number == 10) month = "October";
            if (month_number == 11) month = "November";
            if (month_number == 12) month = "December";
        }

        if(current_UiLanguage == 1)
        {
            if (month_number == 1) month = "Gennaio";
            if (month_number == 2) month = "Febbraio";
            if (month_number == 3) month = "Marzo";
            if (month_number == 4) month = "Aprile";
            if (month_number == 5) month = "Maggio";
            if (month_number == 6) month = "Giugno";
            if (month_number == 7) month = "Luglio";
            if (month_number == 8) month = "Agosto";
            if (month_number == 9) month = "Settembre";
            if (month_number == 10) month = "Ottobre";
            if (month_number == 11) month = "Novembre";
            if (month_number == 12) month = "Dicembre";
        }

        if (current_UiLanguage == 2)
        {
            if (month_number == 1) month = "Enero";
            if (month_number == 2) month = "Febrero";
            if (month_number == 3) month = "Marzo";
            if (month_number == 4) month = "Abril";
            if (month_number == 5) month = "Mayo";
            if (month_number == 6) month = "Junio";
            if (month_number == 7) month = "Julio";
            if (month_number == 8) month = "Agosto";
            if (month_number == 9) month = "Septiembre";
            if (month_number == 10) month = "Octubre";
            if (month_number == 11) month = "Noviembre";
            if (month_number == 12) month = "Diciembre";
        }

        return month;
	}

	/* 
	 * Get Day of Week From Year, Month and Day
	 * 
	 * Example: Monday <===> 2016,1,2
	 */
	string getDayOfWeek(int year, int month, int day)
	{
		System.DateTime dateValue = new System.DateTime(year,month,day);
        string englishName = dateValue.DayOfWeek.ToString();

        if (current_UiLanguage == 0)
            return englishName;
        if (current_UiLanguage == 1)
            switch (englishName)
            {
                case "Monday":
                    return "Lunedì";

                case "Tuesday":
                    return "Martedì";

                case "Wednesday":
                    return "Mercoledì";

                case "Thursday":
                    return "Giovedì";

                case "Friday":
                    return "Venerdì";

                case "Saturday":
                    return "Sabato";

                case "Sunday":
                    return "Domenica";
            }
        else if (current_UiLanguage == 2)
            switch (englishName)
            {
                case "Monday":
                    return "Lunes";

                case "Tuesday":
                    return "Martes";

                case "Wednesday":
                    return "Miércoles";

                case "Thursday":
                    return "Jueves";

                case "Friday":
                    return "Viernes";

                case "Saturday":
                    return "Sábado";

                case "Sunday":
                    return "Domingo";
            }
        return englishName;
    }

    /* 
	 * Get index of first slot where start day numeration
	 */
    int getIndexOfFirstSlotInMonth(int year, int month)
	{
		int indexOfFirstSlot = 0;

		System.DateTime dateValue = new System.DateTime(year,month,1);
		string dayOfWeek          = dateValue.DayOfWeek.ToString();

		if(dayOfWeek == "Monday")    indexOfFirstSlot = 0;
		if(dayOfWeek == "Tuesday")   indexOfFirstSlot = 1;
		if(dayOfWeek == "Wednesday") indexOfFirstSlot = 2;
		if(dayOfWeek == "Thursday")  indexOfFirstSlot = 3;
		if(dayOfWeek == "Friday")    indexOfFirstSlot = 4;
		if(dayOfWeek == "Saturday")  indexOfFirstSlot = 5;
		if(dayOfWeek == "Sunday")    indexOfFirstSlot = 6;

		return indexOfFirstSlot;
	}

	/*
	 * Disable all day slot 
	 */
	void disableAllSlot()
	{
		for(int i = 0; i < max_day_slots; i++)
			disableSlot(i+1);
	}

	/* 
	 * Disable day slot
	 */
	void disableSlot(int numSlot)
	{
        if (!grid.activeInHierarchy)
            return;
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = false;
		day_slot.GetComponent<Image>().enabled  = false;
        day_slot.GetComponentsInChildren<Image>()[1].enabled = false;
        day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = false;
	}

	void setNormalSlot(int numSlot)
    {
        if (!grid.activeInHierarchy)
            return;
        GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = true;
		day_slot.GetComponent<Image>().enabled  = false;
        day_slot.GetComponentsInChildren<Image>()[1].enabled = false;
        day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = true;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_dayTextNormal;
	}

	void setEventSlot(int numSlot)
	{
        if (!grid.activeInHierarchy)
            return;
        GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		day_slot.GetComponent<Button>().enabled = true;
        day_slot.GetComponent<Image>().enabled = false;
        day_slot.GetComponentsInChildren<Image>()[1].enabled  = true;
		day_slot.GetComponentsInChildren<Image>()[1].color    = FlatCalendarStyle2.color_bubbleEvent;
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().enabled = true;
	}

    void populateEventVisualizer()
    {
        List<EventObj> orderedEventsList = getEventList(currentTime.year, currentTime.month, currentTime.day).OrderBy(x => x.time).ToList();
        foreach (Transform child in eventObjcContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        foreach (EventObj e in orderedEventsList)
        {
            var evnt = Instantiate(eventObj, eventObjcContent.transform);
            Button[] btns = evnt.GetComponentsInChildren<Button>();
            foreach(Image img in evnt.GetComponentsInChildren<Image>())
            {
                if (!e.alarm)
                    img.color = new Color32(130, 130, 130, 255);
                else
                    img.color = FlatCalendarStyle2.color_bubbleEvent;
            }
            btns[2].GetComponent<Image>().color = FlatCalendarStyle2.color_topGrid;
            btns[1].GetComponent<Image>().color = FlatCalendarStyle2.color_topGrid;

            btns[2].onClick.AddListener(() => evtListener_ModifyEvent(e));
            btns[1].onClick.AddListener(() => removeEvent(currentTime.year, currentTime.month, currentTime.day, e));
            evnt.GetComponent<Button>().onClick.AddListener(() => ShowEvent(e));
            Text[] t = evnt.GetComponentsInChildren<Text>();
            t[0].text = e.name;
            if (e.time.Value.Hours < 10)
                t[1].text = "0" + e.time.Value.Hours.ToString();
            else
                t[1].text = e.time.Value.Hours.ToString();

            if (e.time.Value.Minutes < 10)
                t[1].text += ":0" + e.time.Value.Minutes.ToString();
            else
                t[1].text += ":" + e.time.Value.Minutes.ToString();

        }
        FlatCalendarStyle2.changeUIStyle(current_UiStyle);

    }

    void populateAllSlot(int monthNumber, int year)
	{
		// Disable all slots
		disableAllSlot();

		// Update slots
		for (int i = 0; i < currentTime.totalDays; i++)
		{	
			// Put text
			changeTextSlot(i+currentTime.dayOffset+1,""+(i+1));

			// Check if slot event
			if(checkEventExist(currentTime.year,currentTime.month,(i+1)))
				setEventSlot(i+currentTime.dayOffset+1);
			else
				setNormalSlot(i+currentTime.dayOffset+1);
		}
	}

	void changeTextSlot(int numSlot, string text)
	{
        if (!grid.activeInHierarchy)
            return;
        GameObject day_slot = GameObject.Find("Slot_"+numSlot);
		day_slot.GetComponent<Button>().GetComponentInChildren<Text>().text = text;
	}

	int getDayInSlot(int numSlot)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (numSlot));
		string txt = day_slot.GetComponentInChildren<Text>().text;
		return int.Parse(txt);
	}

	public void markSelectionDay(int day)
	{
        if (!grid.activeInHierarchy)
            return;
        GameObject day_slot = GameObject.Find("Slot_"+ (day+currentTime.dayOffset));

        day_slot.GetComponent<Image>().enabled = true;

        if(day == DateTime.Now.Day && currentTime.month == DateTime.Now.Month && currentTime.year == DateTime.Now.Year)
        {
            day_slot.GetComponentsInChildren<Image>()[2].enabled = true;
            day_slot.GetComponentsInChildren<Image>()[2].color = FlatCalendarStyle2.color_bubbleSelectionMarker;
            currentDaySlot = day + currentTime.dayOffset;
        }

        day_slot.GetComponent<Image>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        for (int i = 0; i < max_day_slots; i++)
            GameObject.Find("Slot_" + (i + 1)).GetComponentInChildren<Text>().color = new Color32(255, 255, 255, 255);
        day_slot.GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_topGrid;

        // Update Text
        setLabelDayOfWeek();
        label_dayNumber.GetComponent<Text>().text = "" + currentTime.day;
        label_dayNumber.GetComponent<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
    }

    void unmarkSelctionDay(int day)
	{
		GameObject day_slot = GameObject.Find("Slot_"+ (day+currentTime.dayOffset));
        // Change Image
        if (!checkEventExist(currentTime.year,currentTime.month,day))
		{
			setNormalSlot(day+currentTime.dayOffset);
		}
        else
        {
            setEventSlot(day + currentTime.dayOffset);
        }
	}

	public static bool checkEventExist(int year, int month, int day)
	{
		if(events_list == null)
			return false;

		if(!events_list.ContainsKey(year))
			return false;

		if(!events_list[year].ContainsKey(month))
			return false;

		if(!events_list[year][month].ContainsKey(day))
			return false;

		if(events_list[year][month][day] == null)
			return false;

		if(events_list[year][month][day].Count == 0)
			return false;

		return true;
	}

	void addEventsListener()
	{
        btn_SwitchModality.GetComponent<Button>().onClick.AddListener(() => evtListener_SwitchModality());
        btn_nextMonth.GetComponent<Button>().onClick.AddListener(() => evtListener_NextMonth());
		btn_prevMonth.GetComponent<Button>().onClick.AddListener(() => evtListener_PreviousMonth());
        btn_nextYear.GetComponent<Button>().onClick.AddListener(() => evtListener_NextYear());
        btn_prevYear.GetComponent<Button>().onClick.AddListener(() => evtListener_PreviousYear());
        btn_calendar.GetComponent<Button>().onClick.AddListener(()   => evtListener_GoToNowday());
        btn_AddEvent[0].GetComponent<Button>().onClick.AddListener(() => evtListener_ShowAddEvent());
        btn_AddEvent[1].GetComponent<Button>().onClick.AddListener(() => evtListener_ShowAddEvent());
        btn_AddEvent[2].GetComponent<Button>().onClick.AddListener(() => evtListener_ShowAddEvent());
        btn_AddEvent[3].GetComponent<Button>().onClick.AddListener(() => evtListener_onAlarm_Enable_Disable());   
        btn_AddEvent[4].GetComponent<Button>().onClick.AddListener(() => evtListener_AddNewEvent_Detail());   
        btn_Details.GetComponent<Button>().onClick.AddListener(() => evtListener_AddNewEvent_Detail());
        btn_Confirm.GetComponent<Button>().onClick.AddListener(() => evtListener_AddNewEvent());
        event_Visualizer_backButton.GetComponent<Button>().onClick.AddListener(() => evtListenet_VisualizerBackButton());
        for (int i = 0; i < max_day_slots; i++)
			GameObject.Find("Slot_"+(i+1)).GetComponent<Button>().onClick.AddListener(() => evtListener_DaySelected());
	}

	public void setCurrentTime()
	{
		currentTime.year      = System.DateTime.Now.Year;
		currentTime.month     = System.DateTime.Now.Month;
		currentTime.day       = System.DateTime.Now.Day;
		currentTime.dayOfWeek = System.DateTime.Now.DayOfWeek.ToString();
		currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year,currentTime.month);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        //btn_nextYear.GetComponent<Text>().text = (currentTime.year + 1).ToString();
        //btn_prevYear.GetComponent<Text>().text = (currentTime.year - 1).ToString();
        if (startWeekFromSunday)
            currentTime.dayOffset++;
    }

	void setCurrentTime(TimeObj obj)
	{
		obj.year      = System.DateTime.Now.Year;
		obj.month     = System.DateTime.Now.Month;
		obj.day       = System.DateTime.Now.Day;
		obj.dayOfWeek = System.DateTime.Now.DayOfWeek.ToString();
		obj.totalDays = System.DateTime.DaysInMonth(obj.year,obj.month);
		obj.dayOffset = getIndexOfFirstSlotInMonth(obj.year,obj.month);
        if (startWeekFromSunday)
            currentTime.dayOffset++;
    }

	public void installDemoData()
	{
		addEvent(2016,3,7,  new EventObj("Event","Description"));
		addEvent(2016,3,7,  new EventObj("Event","Description"));
		addEvent(2016,3,10, new EventObj("Event","Description"));
		addEvent(2016,3,22, new EventObj("Event","Description"));
		addEvent(2016,4,5,  new EventObj("Event","Description"));
		addEvent(2016,4,5,  new EventObj("Event","Description"));
		addEvent(2016,4,5,  new EventObj("Event","Description"));
		addEvent(2016,4,15, new EventObj("Event","Description"));
		addEvent(2016,4,22, new EventObj("Event","Description"));
		addEvent(2016,5,1,  new EventObj("Event","Description"));
		addEvent(2016,5,2,  new EventObj("Event","Description"));
		addEvent(2016,5,3,  new EventObj("Event","Description"));
		addEvent(2016,5,15, new EventObj("Event","Description"));
		addEvent(2016,6,2,  new EventObj("Event","Description"));
		addEvent(2016,6,3,  new EventObj("Event","Description"));
		addEvent(2016,6,4,  new EventObj("Event","Description"));
		addEvent(2016,6,22, new EventObj("Event","Description"));


		//updateCalendar(currentTime.month,currentTime.year);
		//markSelectionDay(currentTime.day);
	}

	public void setUIStyle(int style)
	{
		current_UiStyle = style;
		FlatCalendarStyle2.changeUIStyle(current_UiStyle);
	}

    public void setLabelDayOfWeek()
    {
        label_dayOfWeek.GetComponent<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;

        // ENGLISH
        if (current_UiLanguage == 0)
        {
            add_event_labels[5].GetComponent<Text>().text = "Today's Events";
            add_event_labels[6].GetComponent<Text>().text = "Today's Events";

            if (!startWeekFromSunday)
            {
                label_dayOfWeek.GetComponent<Text>().text = currentTime.dayOfWeek;
                daysOfWeek_events_label[0].GetComponent<Text>().text = "MON";
                daysOfWeek_events_label[1].GetComponent<Text>().text = "TUE";
                daysOfWeek_events_label[2].GetComponent<Text>().text = "WED";
                daysOfWeek_events_label[3].GetComponent<Text>().text = "THU";
                daysOfWeek_events_label[4].GetComponent<Text>().text = "FRI";
                daysOfWeek_events_label[5].GetComponent<Text>().text = "SAT";
                daysOfWeek_events_label[6].GetComponent<Text>().text = "SUN";
            }
            else
            {
                label_dayOfWeek.GetComponent<Text>().text = currentTime.dayOfWeek;
                daysOfWeek_events_label[1].GetComponent<Text>().text = "MON";
                daysOfWeek_events_label[2].GetComponent<Text>().text = "TUE";
                daysOfWeek_events_label[3].GetComponent<Text>().text = "WED";
                daysOfWeek_events_label[4].GetComponent<Text>().text = "THU";
                daysOfWeek_events_label[5].GetComponent<Text>().text = "FRI";
                daysOfWeek_events_label[6].GetComponent<Text>().text = "SAT";
                daysOfWeek_events_label[0].GetComponent<Text>().text = "SUN";
            }
        }
        // ITALIAN
        if (current_UiLanguage == 1)
        {
            add_event_labels[5].GetComponent<Text>().text = "Eventi Giornalieri";
            add_event_labels[6].GetComponent<Text>().text = "Eventi Giornalieri";

            if (!startWeekFromSunday)
            {
                daysOfWeek_events_label[0].GetComponent<Text>().text = "LUN";
                daysOfWeek_events_label[1].GetComponent<Text>().text = "MAR";
                daysOfWeek_events_label[2].GetComponent<Text>().text = "MER";
                daysOfWeek_events_label[3].GetComponent<Text>().text = "GIO";
                daysOfWeek_events_label[4].GetComponent<Text>().text = "VEN";
                daysOfWeek_events_label[5].GetComponent<Text>().text = "SAB";
                daysOfWeek_events_label[6].GetComponent<Text>().text = "DOM";
            }
            else
            {
                daysOfWeek_events_label[1].GetComponent<Text>().text = "LUN";
                daysOfWeek_events_label[2].GetComponent<Text>().text = "MAR";
                daysOfWeek_events_label[3].GetComponent<Text>().text = "MER";
                daysOfWeek_events_label[4].GetComponent<Text>().text = "GIO";
                daysOfWeek_events_label[5].GetComponent<Text>().text = "VEN";
                daysOfWeek_events_label[6].GetComponent<Text>().text = "SAB";
                daysOfWeek_events_label[0].GetComponent<Text>().text = "DOM";
            }
            switch (currentTime.dayOfWeek)
            {
                case ("Monday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Lunedì";
                    break;
                case ("Tuesday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Martedì";
                    break;
                case ("Wednesday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Mercoledì";
                    break;
                case ("Thursday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Giovedì";
                    break;
                case ("Friday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Venerdì";
                    break;
                case ("Saturday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Sabato";
                    break;
                case ("Sunday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Domenica";
                    break;
            }
        }
        // SPANISH
        if (current_UiLanguage == 2)
        {
            add_event_labels[5].GetComponent<Text>().text = "Eventos de hoy";
            add_event_labels[6].GetComponent<Text>().text = "Eventos de hoy";

            if (!startWeekFromSunday)
            {
                daysOfWeek_events_label[0].GetComponent<Text>().text = "LUN";
                daysOfWeek_events_label[1].GetComponent<Text>().text = "MAR";
                daysOfWeek_events_label[2].GetComponent<Text>().text = "MIÉ";
                daysOfWeek_events_label[3].GetComponent<Text>().text = "JUE";
                daysOfWeek_events_label[4].GetComponent<Text>().text = "VIE";
                daysOfWeek_events_label[5].GetComponent<Text>().text = "SÁB";
                daysOfWeek_events_label[6].GetComponent<Text>().text = "DOM";
            }
            else
            {
                daysOfWeek_events_label[1].GetComponent<Text>().text = "LUN";
                daysOfWeek_events_label[2].GetComponent<Text>().text = "MAR";
                daysOfWeek_events_label[3].GetComponent<Text>().text = "MIÉ";
                daysOfWeek_events_label[4].GetComponent<Text>().text = "JUE";
                daysOfWeek_events_label[5].GetComponent<Text>().text = "VIE";
                daysOfWeek_events_label[6].GetComponent<Text>().text = "SÁB";
                daysOfWeek_events_label[0].GetComponent<Text>().text = "DOM";
            }
                       
            switch (currentTime.dayOfWeek)
            {
                case ("Monday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Lunes";
                    break;
                case ("Tuesday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Martes";
                    break;
                case ("Wednesday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Miércoles";
                    break;
                case ("Thursday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Jueves";
                    break;
                case ("Friday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Viernes";
                    break;
                case ("Saturday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Sábado";
                    break;
                case ("Sunday"):
                    label_dayOfWeek.GetComponent<Text>().text = "Domingo";
                    break;
            }
        }
    }

    public void setLanguageLabelAddEvent()
    {
        if (current_UiLanguage == 0)
        {
            add_event_labels[0].GetComponent<Text>().text = "New Event";
            add_event_labels[1].GetComponent<Text>().text = "Event time";
            add_event_labels[2].GetComponent<Text>().text = "Event name";
            add_event_labels[3].GetComponent<Text>().text = "Event description";
            add_event_labels[4].GetComponent<Text>().text = "Details";
            add_event_labels[7].GetComponent<Text>().text = "Machine";
        }
        if (current_UiLanguage == 1)
        {
            add_event_labels[0].GetComponent<Text>().text = "Nuovo Evento";
            add_event_labels[1].GetComponent<Text>().text = "Orario evento";
            add_event_labels[2].GetComponent<Text>().text = "Nome evento";
            add_event_labels[3].GetComponent<Text>().text = "Descrizione evento";
            add_event_labels[4].GetComponent<Text>().text = "Dettagli";
            add_event_labels[7].GetComponent<Text>().text = "Machina";

        }
        if (current_UiLanguage == 2)
        {
            add_event_labels[0].GetComponent<Text>().text = "Nuevo Evento";
            add_event_labels[1].GetComponent<Text>().text = "Hora del evento";
            add_event_labels[2].GetComponent<Text>().text = "Nombre del evento";
            add_event_labels[3].GetComponent<Text>().text = "Descripción del evento";
            add_event_labels[4].GetComponent<Text>().text = "Detalles";
            add_event_labels[7].GetComponent<Text>().text = "Maquina";
        }
    }

    public void addEvent(int year, int month, int day, EventObj ev)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		events_list[year][month][day].Add(ev);
        refreshCalendar();
	}

    public void modifyEvent(int year, int month, int day, EventObj oldEvent, EventObj newEvent)
    {
        if(checkEventExist(year, month, day))
        {
            if (events_list[year][month][day].Contains(oldEvent))
            {
                removeEvent(year, month, day, oldEvent);
                addEvent(year, month, day, newEvent);
            }
        }
    }

	public void removeEvent(int year, int month, int day, EventObj ev)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		if(events_list[year][month][day].Contains(ev))
			events_list[year][month][day].Remove(ev);

        if (filePath != "")
            exportFlatCalendarEvents(filePath);

        populateEventVisualizer();
        updateCalendar(currentTime.month, currentTime.day);
        event_Visualizer.gameObject.SetActive(false);
    }

    public void removeAllEventOfDay(int year, int month, int day)
	{
		if(!events_list.ContainsKey(year))
			events_list.Add(year,new Dictionary<int, Dictionary<int,List<EventObj>>>());
		
		if(!events_list[year].ContainsKey(month))
			events_list[year].Add(month,new Dictionary<int, List<EventObj>>());
		
		if(!events_list[year][month].ContainsKey(day))
			events_list[year][month].Add(day,new List<EventObj>());

		events_list[year][month][day].Clear();
	}

	public void removeAllCalendarEvents()
	{
		events_list.Clear();
	}

	public static List<EventObj> getEventList(int year, int month, int day)
	{
		List<EventObj> list = new List<EventObj>();

		if(!events_list.ContainsKey(year))
			return list;

		if(!events_list[year].ContainsKey(month))
			return list;

		if(!events_list[year][month].ContainsKey(day))
			return list;

		return events_list[year][month][day];
	}

    void ShowEvent(EventObj evnt)
    {
        if (event_Visualizer.activeInHierarchy)
            event_Visualizer.SetActive(false);
        else
        {
            event_Visualizer.SetActive(true);
            Text[] t = event_Visualizer.GetComponentsInChildren<Text>();
            t[0].text = evnt.name;
            if (evnt.time.Value.Hours < 10)
                t[1].text = "0" + evnt.time.Value.Hours.ToString();
            else
                t[1].text = evnt.time.Value.Hours.ToString();
            if (evnt.time.Value.Minutes < 10)
                    t[1].text += ":0" + evnt.time.Value.Minutes.ToString();
            else
                t[1].text += ":" + evnt.time.Value.Minutes.ToString();
            t[2].text = evnt.description;
            event_Visualizer.GetComponentsInChildren<Button>()[1].onClick.AddListener(() => removeEvent(currentTime.year, currentTime.month, currentTime.day, evnt));
            event_Visualizer.GetComponentsInChildren<Button>()[2].onClick.AddListener(() => evtListener_ModifyEvent(evnt));
        }
    }

    // ================================================
    // =============== BUTTON LISTENERS ===============
    // ================================================

    public void evtListener_SwitchModality()
    {
        if (slider.activeSelf)
        {
            slider.SetActive(false);
            grid.SetActive(true);
            updateCalendar(currentTime.month, currentTime.year);

            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);

            if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
            {
                day_slot.GetComponentsInChildren<Image>()[2].enabled = true;
            }
            else
                day_slot.GetComponentsInChildren<Image>()[2].enabled = false;

            //eventsScrollView.transform.position = new Vector3(eventsScrollView.transform.position.x, eventsScrollView.transform.position.y - 80, eventsScrollView.transform.position.z);
            eventsScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(eventsScrollView.GetComponent<RectTransform>().offsetMax.x, 0);
        }
        else
        {
            slider.SetActive(true);
            grid.SetActive(false);
            
            //eventsScrollView.transform.position = new Vector3(eventsScrollView.transform.position.x, eventsScrollView.transform.position.y + 80, eventsScrollView.transform.position.z);
            eventsScrollView.GetComponent<RectTransform>().offsetMax = new Vector2(eventsScrollView.GetComponent<RectTransform>().offsetMax.x, 150);
            onSliderUpdateCaledar();
        }
    }

	void evtListener_NextMonth()
	{
		unmarkSelctionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = false;
        }

        currentTime.month = (currentTime.month+1) % 13;
		if(currentTime.month == 0)
		{
			currentTime.year++;
			currentTime.month = 1;
		}        

        currentTime.day       = 1;
        currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);
        if (startWeekFromSunday)
            currentTime.dayOffset++;        

        updateCalendar(currentTime.month,currentTime.year);

		markSelectionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = true;
        }

        // Send Callback
        if (delegate_onmonthchanged != null)
			delegate_onmonthchanged(currentTime);
	}

	void evtListener_PreviousMonth()
	{
		unmarkSelctionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = false;
        }

        currentTime.month = (currentTime.month-1) % 13;
		if(currentTime.month == 0)
		{
			currentTime.year--;
			currentTime.month = 12;
		}

		currentTime.day   = 1;
		currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);
		currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year,currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);
        if (startWeekFromSunday)
            currentTime.dayOffset++;

        updateCalendar(currentTime.month,currentTime.year);

		markSelectionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = true;
        }

        // Send Callback
        if (delegate_onmonthchanged != null)
			delegate_onmonthchanged(currentTime);

	}

    void evtListener_NextYear()
    {
        unmarkSelctionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = false;
        }

        currentTime.year += 1;


        currentTime.day = 1;
        currentTime.dayOfWeek = getDayOfWeek(currentTime.year, currentTime.month, currentTime.day);
        currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year, currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);

        if (startWeekFromSunday)
            currentTime.dayOffset++;

        updateCalendar(currentTime.month, currentTime.year);

        markSelectionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = true;
        }

        // Send Callback
        if (delegate_onmonthchanged != null)
            delegate_onmonthchanged(currentTime);
    }

    void evtListener_PreviousYear()
    {
        unmarkSelctionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = false;
        }

        currentTime.year -= 1;


        currentTime.day = 1;
        currentTime.dayOfWeek = getDayOfWeek(currentTime.year, currentTime.month, currentTime.day);
        currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year, currentTime.month);
        currentTime.totalDays = System.DateTime.DaysInMonth(currentTime.year, currentTime.month);

        if (startWeekFromSunday)
            currentTime.dayOffset++;

        updateCalendar(currentTime.month, currentTime.year);

        markSelectionDay(currentTime.day);

        if (DateTime.Now.Month == currentTime.month && DateTime.Now.Year == currentTime.year)
        {
            GameObject day_slot = GameObject.Find("Slot_" + currentDaySlot);
            day_slot.GetComponentsInChildren<Image>()[2].enabled = true;
        }

        // Send Callback
        if (delegate_onmonthchanged != null)
            delegate_onmonthchanged(currentTime);
    }

    public void evtListener_DaySelected()
	{
        // Unmark old slot
		unmarkSelctionDay(currentTime.day);

		// Update current day
		string slot_name             = EventSystem.current.currentSelectedGameObject.name;
		int    slot_position         = int.Parse(slot_name.Substring(5,(slot_name.Length-5)));
		 	   currentTime.day       = getDayInSlot(slot_position);
			   currentTime.dayOfWeek = getDayOfWeek(currentTime.year,currentTime.month,currentTime.day);

		// Mark current slot
		markSelectionDay(currentTime.day);

		// Send Callback
		if(delegate_ondayselected != null)
			delegate_ondayselected(currentTime);

		// Send Callback
		if(getEventList(currentTime.year,currentTime.month,currentTime.day).Count > 0)
			if(delegate_oneventselected != null)
				delegate_oneventselected(currentTime,getEventList(currentTime.year,currentTime.month,currentTime.day));

        populateEventVisualizer();
    }
    public void onSliderDaySelect(GameObject obj)
    {
        foreach (GameObject o in sliderDaysButtons)
        {
            o.GetComponentInChildren<Image>().enabled = false;
        }
        Text[] day_number = obj.GetComponentsInChildren<Text>();
        currentTime.day = Int32.Parse(day_number[0].text);
        onSliderUpdateCaledar();
    }
    public void onSliderMonthSelect(GameObject obj)
    {
        foreach (GameObject o in sliderDaysButtons)
        {
            o.GetComponentInChildren<Image>().enabled = false;
        }
        for (int i = 0; i < 12; i++)
        {
            if (sliderMonthsButtons[i] == obj)
            {
                currentTime.month = i + 1;
                currentTime.day = 1;
                currentTime.totalDays = DateTime.DaysInMonth(currentTime.year, currentTime.month);
                currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year, currentTime.month);
            }
        }
        onSliderUpdateCaledar();
    }
    public void onSliderYearSelect(GameObject obj)
    {
        foreach (GameObject o in sliderDaysButtons)
        {
            o.GetComponentInChildren<Image>().enabled = false;
        }
        for (int i = 0; i < sliderYearsButtons.Length; i++)
        {
            if (sliderYearsButtons[i] == obj)
            {
                currentTime.year = Int32.Parse(sliderYearsButtons[i].GetComponentInChildren<Text>().text);
                currentTime.month = 1;
                currentTime.day = 1;
                currentTime.totalDays = DateTime.DaysInMonth(currentTime.year, currentTime.month);
                currentTime.dayOffset = getIndexOfFirstSlotInMonth(currentTime.year, currentTime.month);
            }
        }
        onSliderUpdateCaledar();
    }

    void evtListener_GoToNowday()
	{
		// Unmark old slot
		unmarkSelctionDay(currentTime.day);

		// Set Current Time
		setCurrentTime();

		// Update Calendar
		updateCalendar(currentTime.month,currentTime.year);

		// Mark Selection Day
		markSelectionDay(currentTime.day);

		// Send Callback
		if(delegate_onnowday != null)
			delegate_onnowday(currentTime);
	}

    void evtListenet_VisualizerBackButton()
    {
        event_Visualizer.SetActive(false);
    }

    void evtListener_ShowAddEvent()
    {
        if (addEvent_Visualizer.gameObject.activeInHierarchy)
        {
            field_timeError.GetComponent<Text>().text = "";
            addEvent_Visualizer.gameObject.SetActive(false);
        }
        else
        {
            addEvent_Visualizer.gameObject.SetActive(true);
            addEvent_Detail_Visualizer.gameObject.SetActive(false);
            label_Date.GetComponent<Text>().text = currentTime.day + "/" + currentTime.month + "/" + currentTime.year;
            label_Date.GetComponentInParent<Image>().color = FlatCalendarStyle2.color_topGrid;
            field_Name.GetComponent<InputField>().text = ""; 
            field_Description.GetComponent<InputField>().text = "";
            setLanguageLabelAddEvent();
            field_Name.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "";
            field_Description.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "";
        }

        currentAlarmValue = true;
        btn_AddEvent[3].GetComponentsInChildren<Image>()[1].color = FlatCalendarStyle2.color_bubbleEvent;
        btn_AddEvent[3].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleEvent;
        btn_AddEvent[3].GetComponentInChildren<Text>().text = "Alarm On";
        selected_hours = -1;
        selected_minutes = -1;
        foreach (Text t in field_TimeSlider[0].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
            t.resizeTextMaxSize = 35;
        }
        foreach (Text t in field_TimeSlider[1].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
            t.resizeTextMaxSize = 35;

        }
    }

    void evtListener_ModifyEvent(EventObj e)
    {
        foreach (Text t in field_TimeSlider[0].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
        }
        foreach (Text t in field_TimeSlider[1].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
        }
        addEvent_Visualizer.gameObject.SetActive(true);
        label_Date.GetComponent<Text>().text = currentTime.day + "/" + currentTime.month + "/" + currentTime.year;
        label_Date.GetComponentInParent<Image>().color = FlatCalendarStyle2.color_topGrid;
        setLanguageLabelAddEvent();
        event_Visualizer.gameObject.SetActive(false);
        field_Name.GetComponent<InputField>().text = e.name;
        add_event_labels[0].GetComponent<Text>().text = e.name;
        field_Description.GetComponent<InputField>().text = e.description;
        selected_hours = e.time.Value.Hours;
        selected_minutes = e.time.Value.Minutes;
        field_TimeSlider[0].GetComponentsInChildren<Text>()[selected_hours].color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        field_TimeSlider[1].GetComponentsInChildren<Text>()[selected_minutes/5].color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        field_Name.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "";
        field_Description.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "";
        modifingEvent = e;
        if (e.alarm != currentAlarmValue)
        {
            evtListener_onAlarm_Enable_Disable();
            //currentAlarmValue = true;
            //btn_AddEvent[3].GetComponentsInChildren<Image>()[1].color = FlatCalendarStyle2.color_bubbleEvent;
            //btn_AddEvent[3].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleEvent;
            //btn_AddEvent[3].GetComponentInChildren<Text>().text = "Alarm On";
        }
    }

    public void evtListener_onAlarm_Enable_Disable()
    {
        if (currentAlarmValue)
        {
            currentAlarmValue = false;
            btn_AddEvent[3].GetComponentsInChildren<Image>()[1].color = new Color32(130, 130, 130, 255);
            btn_AddEvent[3].GetComponentInChildren<Text>().color = new Color32(130, 130, 130, 255);
            btn_AddEvent[3].GetComponentInChildren<Text>().text = "Alarm Off";

        }
        else
        {
            currentAlarmValue = true;
            btn_AddEvent[3].GetComponentsInChildren<Image>()[1].color = FlatCalendarStyle2.color_bubbleEvent;
            btn_AddEvent[3].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleEvent;
            btn_AddEvent[3].GetComponentInChildren<Text>().text = "Alarm On";

        }
    }

    void evtListener_AddNewEvent_Detail()
    {
        if (addEvent_Detail_Visualizer.gameObject.activeInHierarchy)
        {
            field_timeError.GetComponent<Text>().text = "";
            addEvent_Detail_Visualizer.gameObject.SetActive(false);
        }
        else
        {
            #region VALIDACIONES 
            TimeSpan new_time = new TimeSpan(selected_hours, selected_minutes, 00);
            string name = field_Name.GetComponent<InputField>().text;
            string description = field_Description.GetComponent<InputField>().text;
            if (name == "")
            {
                string missing_name = "";
                if (current_UiLanguage == 0)
                    missing_name = "Event name is required!";
                if (current_UiLanguage == 1)
                    missing_name = "Nome evento necessario!";
                if (current_UiLanguage == 2)
                    missing_name = "Nombre del evento es obligatorio!";

                field_Name.GetComponent<InputField>().placeholder.GetComponent<Text>().text = missing_name;
                Debug.Log("Name is Required");
                return;
            }
            if (description == "")
            {
                string missing_name = "";
                if (current_UiLanguage == 0)
                    missing_name = "Event description is required!";
                if (current_UiLanguage == 1)
                    missing_name = "Descrizione evento necessaria!";
                if (current_UiLanguage == 2)
                    missing_name = "Se requiere descripción del evento!";

                field_Description.GetComponent<InputField>().placeholder.GetComponent<Text>().text = missing_name;
                Debug.Log("Description is Required");
                return;
            }
            if(selected_hours == -1 || selected_minutes == -1)
            {
                string missing_time = "";
                if (current_UiLanguage == 0)
                    missing_time = "*Event time is required!";
                if (current_UiLanguage == 1)
                    missing_time = "*Orario evento necessario!";
                if (current_UiLanguage == 2)
                    missing_time = "*Se requiere tiempo de evento!";
                Debug.Log("Time is Required");
                field_timeError.GetComponent<Text>().text = missing_time;
                return;
            }
            #endregion
            //TODO Cargue place holder y campos por defecto
            addEvent_Detail_Visualizer.gameObject.SetActive(true);
            label_Date_Detail.GetComponent<Text>().text = currentTime.day + "/" + currentTime.month + "/" + currentTime.year;
            label_Date_Detail.GetComponentInParent<Image>().color = FlatCalendarStyle2.color_topGrid;
            field_User.GetComponent<InputField>().text = ""; 
            field_Machine.GetComponent<InputField>().text = "";
            setLanguageLabelAddEvent();
            field_User.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "";
            field_Machine.GetComponent<InputField>().placeholder.GetComponent<Text>().text = "";
        }
    }

    void evtListener_AddNewEvent()
    {
        TimeSpan new_time = new TimeSpan(selected_hours, selected_minutes, 00);
        string name = field_Name.GetComponent<InputField>().text;
        string description = field_Description.GetComponent<InputField>().text;
        if (name == "")
        {
            string missing_name = "";
            if (current_UiLanguage == 0)
                missing_name = "Event name is required!";
            if (current_UiLanguage == 1)
                missing_name = "Nome evento necessario!";
            if (current_UiLanguage == 2)
                missing_name = "Nombre del evento es obligatorio!";

            field_Name.GetComponent<InputField>().placeholder.GetComponent<Text>().text = missing_name;
            Debug.Log("Name is Required");
            return;
        }
        if (description == "")
        {
            string missing_name = "";
            if (current_UiLanguage == 0)
                missing_name = "Event description is required!";
            if (current_UiLanguage == 1)
                missing_name = "Descrizione evento necessaria!";
            if (current_UiLanguage == 2)
                missing_name = "Se requiere descripción del evento!";

            field_Description.GetComponent<InputField>().placeholder.GetComponent<Text>().text = missing_name;
            Debug.Log("Description is Required");
            return;
        }
        if(selected_hours == -1 || selected_minutes == -1)
        {
            string missing_time = "";
            if (current_UiLanguage == 0)
                missing_time = "*Event time is required!";
            if (current_UiLanguage == 1)
                missing_time = "*Orario evento necessario!";
            if (current_UiLanguage == 2)
                missing_time = "*Se requiere tiempo de evento!";
            Debug.Log("Time is Required");
            field_timeError.GetComponent<Text>().text = missing_time;
            return;
        }
        EventObj evnt = new EventObj(name, description, new_time);
        evnt.alarm = currentAlarmValue;
        var existingEvent = getEventList(currentTime.year, currentTime.month, currentTime.day).Where(item => item.name == evnt.name).ToList();

        //event already exists
        if(modifingEvent != null)
        {
            modifyEvent(currentTime.year, currentTime.month, currentTime.day, modifingEvent, evnt);
        }
        else
            addEvent(currentTime.year, currentTime.month, currentTime.day, evnt);

        modifingEvent = null;
        addEvent_Visualizer.gameObject.SetActive(false);
        //event_Visualizer.gameObject.SetActive(false);
        markSelectionDay(currentTime.day);
        sliderDaysButtons[currentTime.day-1].GetComponentInChildren<Image>().enabled = true;
        if (filePath != "")
            exportFlatCalendarEvents(filePath);
        updateCalendar(currentTime.month, currentTime.day);
        foreach (Text t in field_TimeSlider[0].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
        }
        foreach (Text t in field_TimeSlider[1].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
        }
        field_timeError.GetComponent<Text>().text = "";
        if (!currentAlarmValue)
        {
            currentAlarmValue = true;
            btn_AddEvent[3].GetComponentsInChildren<Image>()[1].color = FlatCalendarStyle2.color_bubbleEvent;
            btn_AddEvent[3].GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleEvent;
            btn_AddEvent[3].GetComponentInChildren<Text>().text = "Alarm On";
        }
    }
    public void onSliderHoursSelect(GameObject obj)
    {
        foreach (Text t in field_TimeSlider[0].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
            t.resizeTextMaxSize = 35;
        }
        obj.GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        obj.GetComponentInChildren<Text>().resizeTextMaxSize = obj.GetComponentInChildren<Text>().resizeTextMaxSize + 10;

        selected_hours = Int32.Parse(obj.GetComponentInChildren<Text>().text);
    }
    public void onSliderMinutesSelect(GameObject obj)
    {
        foreach (Text t in field_TimeSlider[1].GetComponentsInChildren<Text>())
        {
            t.color = new Color32(156, 156, 156, 255);
            t.resizeTextMaxSize = 35;
        }
        obj.GetComponentInChildren<Text>().color = FlatCalendarStyle2.color_bubbleSelectionMarker;
        obj.GetComponentInChildren<Text>().resizeTextMaxSize = obj.GetComponentInChildren<Text>().resizeTextMaxSize+10;

        selected_minutes = Int32.Parse(obj.GetComponentInChildren<Text>().text);
    }

    void evtListener_UpdateEventsFile()
    {
        if (filePath != "")
            exportFlatCalendarEvents(filePath);
    }

    // =========================================================
    // ================= SET DELEGATE CALLBACKS ================
    // =========================================================

    public void setCallback_OnDaySelected(Delegate_OnDaySelected func)
	{
		delegate_ondayselected = func;
	}

	public void setCallback_OnEventSelected(Delegate_OnEventSelected func)
	{
		delegate_oneventselected = func;
	}

	public void setCallback_OnMonthChanged(Delegate_OnMonthChanged func)
	{
		delegate_onmonthchanged = func;
	}

	public void setCallback_OnNowday(Delegate_OnNowDay func)
	{
		delegate_onnowday = func;
	}

    public void setCallback_OnTriggerEvent(Delegate_OnTriggerEvent func)
    {
        delegate_ontriggerevent = func;
    }

}

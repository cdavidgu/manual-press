/**
 * Flat Calendar
 * 
 * This class manage Flat Calendar Themes
 *
 * @version 2.0
 * @author  Gerardo Ritacco, Aldo Meo
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

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FlatCalendarStyle2
{
	/*
	 * UI Colors Configuration 
	 */
	public enum COLORS_TYPE { GREEN, RED, PINK, BLUE};
	/*
	 * Single UI Colors Items
	 */
	public static Color32 color_slideHeader;
	public static Color32 color_slideSubHeader;
	public static Color32 color_slideBody;

	public static Color32 color_topGrid;

	public static Color32 color_dayTextNormal;
    public static Color32 color_dayTextNormalGrid;
    public static Color32 color_bubbleEvent;
    public static Color32 color_bubbleSelectionMarker;
    public static Color32 color_EventObj;

    public static void changeUIStyle(int style)
	{

		// ==================================================
		// =================== GREEN ========================
		// ==================================================
		if(style == (int) FlatCalendarStyle2.COLORS_TYPE.GREEN)
		{
            color_slideHeader           = new Color32(92, 103, 80, 255);
            color_slideSubHeader        = new Color32(111, 122, 99, 255);
            color_slideBody             = new Color32( 134, 146, 121, 255);

            color_topGrid = new Color32(92, 103, 80, 255);

            color_dayTextNormal = new Color32(255, 255, 255, 255);
            color_dayTextNormalGrid = new Color32(177, 182, 171, 255);

            color_bubbleEvent = new Color32(217, 143, 107, 255);
            color_bubbleSelectionMarker = new Color32(231, 199, 148, 255);
            color_EventObj = new Color32(223, 224, 196, 225);
        }

		// ===============================================
		// =================== RED =======================
		// ===============================================
		if(style == (int) FlatCalendarStyle2.COLORS_TYPE.RED)
		{
            color_slideHeader = new Color32(118, 41, 41, 255);
            color_slideSubHeader = new Color32(142, 57, 57, 255);
            color_slideBody = new Color32(161, 68, 68, 255);

            color_topGrid = new Color32(118, 42, 42, 255);

            color_dayTextNormal = new Color32(255, 255, 255, 255);
            color_dayTextNormalGrid = new Color32(208, 161, 161, 255);

            color_bubbleEvent = new Color32(188, 188, 54, 255);
            color_bubbleSelectionMarker = new Color32(244, 209, 187, 255);
            color_EventObj = new Color32(243, 233, 227, 255);
            
        }

        // ==============================================
        // =================== PINK =====================
        // ==============================================
        if (style == (int) FlatCalendarStyle2.COLORS_TYPE.PINK)
		{
            color_slideHeader = new Color32(193, 146, 154, 255);
            color_slideSubHeader = new Color32(201, 165, 171, 255);
            color_slideBody = new Color32(209, 184, 188, 255);

            color_topGrid = new Color32(193, 146, 154, 255);

            color_dayTextNormal = new Color32(255, 255, 255, 255);
            color_dayTextNormalGrid = new Color32(222, 204, 207, 255);

            color_bubbleEvent = new Color32(100, 143, 78, 255);
            color_bubbleSelectionMarker = new Color32(105, 49, 50, 255);
            color_EventObj = new Color32(201, 165, 171, 98);
        }

        // ===========================================
        // =================== BLUE ==================
        // ===========================================
        if (style == (int) FlatCalendarStyle2.COLORS_TYPE.BLUE)
		{
            color_slideHeader = new Color32(3, 68, 73, 255);
            color_slideSubHeader = new Color32(4, 91, 98, 255);
            color_slideBody = new Color32(5, 114, 122, 255);

            color_topGrid = new Color32(3, 68, 83, 255);

            color_dayTextNormal = new Color32(255, 255, 255, 255);
            color_dayTextNormalGrid = new Color32(123, 167, 171, 255);

            color_bubbleEvent = new Color32(210, 120, 15, 255);
            color_bubbleSelectionMarker = new Color32(228, 173, 67, 255);
            color_EventObj = new Color32(220, 230, 230, 120);
        }

        /* Change Colors */
        if (GameObject.Find("SlideHeader") != null)
            GameObject.Find("SlideHeader").GetComponent<Image>().color       = color_slideHeader;

        if (GameObject.Find("SlideSubHeader") != null)
            GameObject.Find("SlideSubHeader").GetComponent<Image>().color    = color_slideSubHeader;

		if(GameObject.Find("SlideBody") != null)
            GameObject.Find("SlideBody").GetComponent<Image>().color         = color_slideBody;

        if (GameObject.Find("Top_Grid") != null) 
            GameObject.Find("Top_Grid").GetComponent<Image>().color          = color_topGrid;

        if (GameObject.Find("Calendar_Btn") != null)
            GameObject.Find("Calendar_Btn").GetComponent<Image>().color      = color_bubbleSelectionMarker;

        if (GameObject.Find("Slide_Grid_Switch_Button") != null)
            GameObject.Find("Slide_Grid_Switch_Button").GetComponent<Image>().color = color_bubbleSelectionMarker;

        if(GameObject.Find("Grid_Slide_Switch_Button") != null)
            GameObject.Find("Grid_Slide_Switch_Button").GetComponent<Image>().color = color_bubbleSelectionMarker;

        if (GameObject.Find("grid_slide_center") != null)
            GameObject.Find("grid_slide_center").GetComponent<Image>().color = color_bubbleEvent;

        if (GameObject.Find("slide_grid_center") != null)
            GameObject.Find("slide_grid_center").GetComponent<Image>().color = color_bubbleEvent;

        if (GameObject.Find("Add_Event_Button") != null)
            GameObject.Find("Add_Event_Button").GetComponent<Image>().color = color_bubbleSelectionMarker;

        if (GameObject.Find("Add_Event_Button2") != null)
            GameObject.Find("Add_Event_Button2").GetComponent<Image>().color = color_bubbleSelectionMarker;

        if (GameObject.Find("Hours Scroll View") != null)
            GameObject.Find("Hours Scroll View").GetComponent<Image>().color = color_slideHeader;

        if (GameObject.Find("Minutes Scroll View") != null)
            GameObject.Find("Minutes Scroll View").GetComponent<Image>().color = color_slideSubHeader;

        if (GameObject.Find("EventsContent") != null)
        {
            foreach (RawImage image in GameObject.Find("EventsContent").GetComponentsInChildren<RawImage>())
                image.color = color_EventObj;
        }

        if (GameObject.Find("HoursScrollbar") != null)
            GameObject.Find("HoursScrollbar").GetComponent<Image>().color = color_slideHeader;

        if (GameObject.Find("MinutesScrollbar") != null)
            GameObject.Find("MinutesScrollbar").GetComponent<Image>().color = color_slideHeader;

        if (GameObject.Find("Delete") != null)
            GameObject.Find("Delete").GetComponent<Image>().color = color_topGrid;

        if (GameObject.Find("alarm button") != null)
        {
            GameObject.Find("alarm button").GetComponentsInChildren<Image>()[1].color = color_bubbleEvent;
            GameObject.Find("alarm button").GetComponentInChildren<Text>().color = color_bubbleEvent;
        }

        if (GameObject.Find("Edit") != null)
            GameObject.Find("Edit").GetComponent<Image>().color = color_topGrid;

        if (GameObject.Find("+") != null)
            GameObject.Find("+").GetComponent<Image>().color = color_topGrid;

    }
}

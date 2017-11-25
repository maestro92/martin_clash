using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;

public enum ClientDebugPanelTab
{
    NetPerf,
};


public enum ClientMiniNetworkMeterMode
{
    Small,
    Med,
    Large,
    XLarge,

};





public class ClientDebugPanel
{
    // a function pointer
    public delegate void    ButtonClickedCallback( object voidObjectDataIn );
    private UnityEngine.Texture2D   m_texture2DBackground                               = null;

    public const float      VirtualScreenWidth      = 800.0f;
    public const float      VirtualScreenHeight     = 1200.0f;

    public bool showNetPerfPanel;
    public bool showLivePingFPSOverlay;
    public bool showMiniNetworkMeter;

    private GameClient m_localGameClient;

    private UnityEngine.Texture2D m_hasInitNetMeterTextures;
    private UnityEngine.Texture2D m_netMeterTexture2DBackgroundDisconnected = null;
    private UnityEngine.Texture2D m_netMeterTexture2DBackgroundConnected = null;
    private UnityEngine.Texture2D m_netMeterTexture2DSecondBorderMarker = null;
    private UnityEngine.Texture2D m_netMeterTexture2DServerData = null;
    private UnityEngine.Texture2D m_netMeterTexture2DClientData = null;
    private UnityEngine.Texture2D m_netMeterTexture2DGreyData = null;
    private UnityEngine.Texture2D m_netMeterTexture2DClippedData = null;



    public ClientDebugPanel()
    {
        showNetPerfPanel = false;
        showLivePingFPSOverlay = false;
        showMiniNetworkMeter = false;

        m_texture2DBackground = PrivateCreateTexture2DColor( 2, 2, new Color(0.5f, 0.5f, 0.5f, 0.9f )); // grey (semi-transparent)


        // we make the background transparent dark blue once connected
        m_netMeterTexture2DBackgroundDisconnected = PrivateCreateTexture2DColor( 2, 2, new Color(0.2f, 0.2f, 0.2f, 0.8f) ); // dark grey (semi-transparent)
        m_netMeterTexture2DBackgroundConnected = PrivateCreateTexture2DColor( 2, 2, new Color(0.0f, 0.0f, 0.5f, 0.8f) );     // dark blue (semi-transparent)
        m_netMeterTexture2DSecondBorderMarker = PrivateCreateTexture2DColor( 2, 2, new Color(1.0f, 1.0f, 1.0f, 1.0f) );      // white
        m_netMeterTexture2DServerData = PrivateCreateTexture2DColor( 2, 2, new Color(0.0f, 1.0f, 0.0f, 1.0f) );             // green
        m_netMeterTexture2DClientData = PrivateCreateTexture2DColor( 2, 2, new Color(1.0f, 1.0f, 0.0f, 1.0f) );             // yellow
        m_netMeterTexture2DGreyData = PrivateCreateTexture2DColor( 2, 2, new Color(0.5f, 0.5f, 0.5f, 1.0f) );               // grey
        m_netMeterTexture2DClippedData = PrivateCreateTexture2DColor( 2, 2, new Color(1.0f, 0.0f, 0.0f, 1.0f) );            // red
    }


    public void SetLocalGameClient(GameClient gameClientIn)
    {
        m_localGameClient = gameClientIn;
    }

    private static Texture2D PrivateCreateTexture2DColor( int widthIn, int heightIn, Color color )
    {
        Texture2D texture2D = new Texture2D( widthIn, heightIn );
        PrivateTexture2DFillWithColor( texture2D, color );
        return texture2D;
    }

    private static void PrivateTexture2DFillWithColor( Texture2D texture2dIn, Color color )
    {
        if( texture2dIn == null )
        {
            return;
        }

        for( int y = 0; y < texture2dIn.height; y++ )
        {
            for ( int x = 0; x < texture2dIn.width; x++ )
            {
                texture2dIn.SetPixel( x, y, color );
            }
        }
        texture2dIn.Apply();
    }

    public void RenderShowButton()
    {
        // add disable button
        float but_w = VirtualScreenWidth * 0.25f;
        float but_h = VirtualScreenHeight * 0.05f;
        float but_x = ( VirtualScreenWidth * 0.5f ) - ( but_w * 0.5f );
        float but_y = 0.0f;

        if (showNetPerfPanel == true)
        {
            AddButton(but_x, but_y, but_w, but_h, Color.white, "Close", this, new ButtonClickedCallback(delegate( object voidObjectDataIn)
                    {
                        ClientDebugPanel clientDebugPanel = voidObjectDataIn as ClientDebugPanel;
                        if (clientDebugPanel != null)
                        {
                            showNetPerfPanel = false;
                        }
                    }));
        }
        else
        {
            AddButton(but_x, but_y, but_w, but_h, Color.white, "Open", this, new ButtonClickedCallback(delegate( object voidObjectDataIn)
                {
                    ClientDebugPanel clientDebugPanel = voidObjectDataIn as ClientDebugPanel;
                    if (clientDebugPanel != null)
                    {
                        showNetPerfPanel = true;
                    }
                }));
        }
    }


    public void Render()
    {

        RenderShowButton();

        if(showLivePingFPSOverlay)
        {
            RenderLivePingFPSOverlay();
        }

        if (showNetPerfPanel == true)
        {
            RenderNetPerfPanel();
            AddText(0, 0, 200, 200, Color.white, "Nice", TextAnchor.UpperLeft, 32);
        }
        else
        {
        
        }
    }

    private void RenderLivePingFPSOverlay()
    {
        float panelWidth = 90.0f;
        float panelHeight = 24.0f + 24.0f + 10;
        float panelUlx = VirtualScreenWidth - panelWidth - 5;
        float panelUly = 10.0f;
        Color colorPanel = new Color(0.0f, 0.0f, 0.0f, 1.0f);
        AddPanel(panelUlx, panelUly, panelWidth, panelHeight, colorPanel);
    }


    private void RenderNetPerfPanel()
    {
        float panelWidth = VirtualScreenWidth * 0.99f;
        float panelHeight = VirtualScreenHeight * 0.9f;
        float panelUlx = (VirtualScreenWidth * 0.5f) - (panelWidth * 0.5f);
        float panelUly = (VirtualScreenHeight * 0.5f) - (panelHeight * 0.5f);
        Color colorPanel = new Color(0.25f, 0.25f, 0.25f, 1.0f);
        AddPanel(panelUlx, panelUly, panelWidth, panelHeight, colorPanel);

        float x = 0;
        float y = panelUly;
        float w = 500;
        float dy = 24;

        // g stands for gap
        float gx = 10.0f;
        float gy = 6.0f;

        Color panelColor = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        float meterX = panelWidth - 2 * gx;
        float meterY = 150;

        if (NetGlobal.netMeter != null && NetGlobal.netMeter.IsEnabled())
        {
            RenderNetMeterPanel(NetGlobal.netMeter, 0, y, meterX, meterY, panelColor);
        }
        x = 0;
        y += (meterY + gy + gy);


        // row 1
        AddText( x, y, w, dy, Color.white, "Lag Simulator", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddButton( x, y, w, dy, Color.white, "Hiccup", this, new ButtonClickedCallback(delegate( object voidObjectDataIn)
            {
                ClientDebugPanel clientDebugPanel = voidObjectDataIn as ClientDebugPanel;
                if (clientDebugPanel != null)
                {
                    // hiccup
                    Debug.LogError("[NEED to Implement Hiccup]");
                }
            }));
                
        x = 0;
        y += (dy + gy);



        // row 2
        AddText( x, y, w, dy, Color.white, "Ping", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, m_localGameClient.connection.pingHelper.GetPing().ToString() + "ms");

        x = 0;
        y += (dy + gy);



        // row 3
        AddText( x, y, w, dy, Color.white, "Receive Clump Quanta", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to put an EDITOR BOX here]");

        x = 0;
        y += (dy + gy);




        // row 6
        AddText( x, y, w, dy, Color.white, "Computed SW", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, m_localGameClient.rateSmoother.GetNumFramesToBufferAhead().ToString());

        x = 0;
        y += (dy + gy);

        // row 5: empty line
        y += (dy + gy);

        // row 6
        AddText( x, y, w, dy, Color.white, "Head", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, m_localGameClient.rateSmoother.GetFrameHead().ToString());
        x = 0;
        y += (dy + gy);

        // row 7
        AddText( x, y, w, dy, Color.white, "Tail", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, m_localGameClient.rateSmoother.GetFrameTail().ToString());
        x = 0;
        y += (dy + gy);


        // row 8
        AddText( x, y, w, dy, Color.white, "Diff + ConsumerCounter", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, (m_localGameClient.rateSmoother.GetFrameDiff()).ToString() + " + " + m_localGameClient.rateSmoother.GetFrameConsumeCounter().ToString());
        x = 0;
        y += (dy + gy);

        AddSmoothingWindowAnalyserGraph( x, y, panelWidth - ( 2 * gx ), 90, null, 2000.0f, 500.0f );
    }



    public void AddPanel( float ulxIn, float ulyIn, float widthIn, float heightIn, UnityEngine.Color colorIn )
    {
        GUI.color = colorIn;
        GUI.contentColor = colorIn;
        GUI.skin.box.normal.background = m_texture2DBackground;
        RectOffset rectOffsetOld = GUI.skin.box.border;
        GUI.skin.box.border = new RectOffset( 0, 0, 0, 0 );
        GUI.Box( new Rect( ulxIn, ulyIn, widthIn, heightIn ), GUIContent.none);
        GUI.skin.box.border = rectOffsetOld;
    }

    public void AddText( Rect rectIn, Color colorIn, string stringIn, TextAnchor textAnchorIn = TextAnchor.UpperLeft, int fontSizeIn = 24 )
    {
        GUI.skin.label.fontSize = fontSizeIn;
        GUI.skin.label.border.top = 0;
        GUI.skin.label.border.left = 0;

        GUI.color = colorIn;
        GUI.contentColor = Color.black;

        GUIStyle guiStyle = GUI.skin.GetStyle( "Label" );
        guiStyle.alignment = textAnchorIn;

        GUI.Label( new Rect( rectIn.x+2, rectIn.y-6, rectIn.width, rectIn.height+24 ), stringIn, guiStyle );
        GUI.contentColor = colorIn;
        GUI.Label( new Rect( rectIn.x, rectIn.y-6, rectIn.width, rectIn.height+24 ), stringIn, guiStyle );
    }

    public void AddText( float ulxIn, float ulyIn, float widthIn, float heightIn, Color colorIn, string stringIn, TextAnchor textAnchorIn = TextAnchor.UpperLeft, int fontSizeIn = 24 )
    {
        AddText( new Rect (ulxIn, ulyIn, widthIn, heightIn), colorIn, stringIn, textAnchorIn, fontSizeIn );
    }

    public void AddButton( Rect rectIn, Color colorIn, string labelIn, object voidObjectDataIn, ButtonClickedCallback buttonClickedCallbackIn, int fontSizeIn = 16 )
    {
        GUI.color = colorIn;
        GUI.contentColor = colorIn;
        GUIStyle customButton = new GUIStyle("button");
        customButton.fontSize = fontSizeIn;
        if( GUI.Button( rectIn, labelIn, customButton ) )
        {
            if( buttonClickedCallbackIn != null )
            {
                buttonClickedCallbackIn( voidObjectDataIn );
            }
        }
    }
        
    public void AddButton( float ulxIn, float ulyIn, float widthIn, float heightIn, Color colorIn, string labelIn, object voidObjectDataIn, ButtonClickedCallback buttonClickedCallbackIn, int fontSizeIn = 16 )
    {
        AddButton( new Rect( ulxIn, ulyIn, widthIn, heightIn ), colorIn, labelIn, voidObjectDataIn, buttonClickedCallbackIn, fontSizeIn );
    }


    private void RenderNetMeterPanel(NetMeter netMeterIn, float xIn, float yIn, float wIn, float hIn, Color color)
    {
        AddPanel(xIn, yIn, wIn, hIn, color);

        netMeterIn.Pump();

        float gy = 6.0f;
        float h = hIn / 2 - gy;
                    
        float y1 = yIn;
        float y2 = yIn + h + gy;

        GUIStyle guiStyleFont = new GUIStyle( GUI.skin.label );
        guiStyleFont.fontSize = (int) ( 48 * 0.5f );

        string sendLabel = "Send";
        string receiveLabel = "Receive";


        RenderMeter(netMeterIn, sendLabel, NetMeter.EntryFlag.Send, new Rect(xIn, y1, wIn, h), guiStyleFont);
        RenderMeter(netMeterIn, receiveLabel, NetMeter.EntryFlag.Receive, new Rect(xIn, y2, wIn, h), guiStyleFont);


    }



    // here we are marking every second
    // the idea is that we first find the starttime
    // you can see that timeStampSecMarker is initialized with the starting time
    // then we quantize it up to the nearest factor of 1000, which is the nearest "sec" mark

    // so if your starting time was 1232, we quantize it up, this beocomes 2000
    private void RenderMeterSecMarkers(NetMeter netMeterManagerIn, 
                                        Int64 msTimeStart, Int64 msTimeEnd, Int64 msTimeSpan, Rect panelRect)
    {
        Int64 timeStampSecMarker = msTimeStart;       

        // we find the nearest (larger or equal) 1 sec mark 
        timeStampSecMarker = NetUtil.QuantizeUpToNearestMS(timeStampSecMarker, 1000);

        GUI.skin.box.normal.background = m_netMeterTexture2DSecondBorderMarker;

        while (timeStampSecMarker < msTimeEnd)
        {
            float xPercent = (timeStampSecMarker - msTimeStart) / (float)msTimeSpan;
            float xStart = panelRect.x + panelRect.width * xPercent;
            GUI.Box( new Rect( xStart, panelRect.y, 1, panelRect.height ), GUIContent.none );

            timeStampSecMarker += 1000;
        }            
    }

    // we render the header and numKBytesThisSec Label
    private void RenderNetMeterLabels(string labelIn, float numBytesThisSec, Rect panelRect, GUIStyle guiStyleFontIn)
    {
        float kPerSec = ( numBytesThisSec / 1024 );

        Color labelTextColor;
        // rendering the label

        float w = 150;
        float h = 50;
        Rect rect = new Rect();
        rect.x = panelRect.x + 10;
        rect.y = panelRect.y;
        rect.width = w;
        rect.height = h;

        // draw bps
        if( kPerSec > 10.0f )       // more than 10K/sec, yikes!
        {
            labelTextColor = UnityEngine.Color.red;
        }
        else if( kPerSec > 5.0f )   // more than 5K/sec, might be dicey
        {
            labelTextColor = UnityEngine.Color.yellow;
        }
        else                        // < 5K/sec, nice!
        {
            labelTextColor = UnityEngine.Color.white;
        }

        string kbps = string.Format( "{0:F2} K/s", kPerSec );

        GUI.contentColor = labelTextColor;
        GUI.Label( rect, labelIn );


        // rendering the kbps
        rect.x = panelRect.x + panelRect.width - w;
        rect.y = panelRect.y;
        rect.width = w;
        rect.height = h;

        GUI.contentColor = labelTextColor;
        GUI.Label(rect, kbps + "/ kbps");
    }

    private void RenderMeter(NetMeter netMeterIn, string labelIn, NetMeter.EntryFlag entryFlagIn, Rect panelRect, GUIStyle guiStyleFontIn)
    {
        // draw the background
        if (Main.instance.mainGameClient.connection.IsConnected() == true)
        {
            GUI.skin.box.normal.background = m_netMeterTexture2DBackgroundConnected;
        }
        else
        {
            GUI.skin.box.normal.background = m_netMeterTexture2DBackgroundDisconnected;
        }

        GUI.Box( panelRect, GUIContent.none );

        float numBytesTotal = 0.0f;

        GUI.skin.box.border.top = 0;
        GUI.skin.box.border.bottom = 0;
        GUI.skin.box.border.left = 0;
        GUI.skin.box.border.right = 0;

        Int64 msTimeEnd = Util.GetRealTimeMS();
        Int64 msTimeStart = msTimeEnd - netMeterIn.GetCaptureWindowSizeMS();
        Int64 msTimeSpan = netMeterIn.GetCaptureWindowSizeMS();
        // BPS: bytes per/sec. We will record the bytes persecond here
        // so we set the start time 1000 ms (which is 1 sec) before the msTimeEnd
        Int64 msTimeBPStart = msTimeEnd - 1000;

        RenderMeterSecMarkers(netMeterIn, msTimeStart, msTimeEnd, msTimeSpan, panelRect);

        float numBytesThisSec = 0.0f;
        lock (NetGlobal.netMeter)
        {
            if (netMeterIn.EntryList.Count > 0)
            {
                float maxY = netMeterIn.GetNumVerticalBytesInMeter();

                //   Debug.LogError("netMeterIn.EntryList " + netMeterIn.EntryList.Count.ToString());
                //   Debug.LogError("msTimeStart " + msTimeStart.ToString());
                //   Debug.LogError("msTimeEnd " + msTimeEnd.ToString());
            //    Debug.LogError(">>>> RenderMeter Start");
                foreach (var entry in netMeterIn.EntryList)
                {
                    if ((entry.GetEntryFlag() & entryFlagIn) == entryFlagIn)
                    {
                        Int64 timeStamp = entry.GetTimeStamp();
                        if (msTimeStart <= timeStamp && timeStamp <= msTimeEnd)
                        {
                            bool exceededYMax = false;
                            // start rendering
                            float xPercent = (timeStamp - msTimeStart) / (float)msTimeSpan;
                            float dataX = panelRect.x + panelRect.width * xPercent;

                            float dataW = (netMeterIn.GetQuantizeToNearestMS() / (float)msTimeSpan) * panelRect.width;

                            float hPercent = entry.GetNumBytes() / maxY;

                            if (hPercent > 1.0f)
                            {
                                hPercent = 1.0f;
                                exceededYMax = true;
                            }

                            float dataH = panelRect.height * hPercent;
                            float dataY = panelRect.y + panelRect.height - dataH;

                            if (exceededYMax == true)
                            {
                                GUI.skin.box.normal.background = m_netMeterTexture2DClippedData;
                            }
                            else if (entry.IsEntryFlagSet(NetMeter.EntryFlag.Client))
                            {
                                GUI.skin.box.normal.background = m_netMeterTexture2DClientData;
                            }
                            else if (entry.IsEntryFlagSet(NetMeter.EntryFlag.Server))
                            {
                                GUI.skin.box.normal.background = m_netMeterTexture2DServerData;
                            }

                            Rect newRect = new Rect(dataX, dataY, dataW, dataH);
                            GUI.Box(newRect, GUIContent.none);

                            if ((msTimeEnd - 1000) <= timeStamp && timeStamp <= msTimeEnd) // within the last second
                            {
                                numBytesThisSec += entry.GetNumBytes();
                            }
                        }
                    }
                }
            //    Debug.LogError(">>>> RenderMeter End");
            }
        }
        RenderNetMeterLabels(labelIn, numBytesThisSec, panelRect, guiStyleFontIn);
    }


    private void AddSmoothingWindowAnalyserGraph( float ulxIn, float ulyIn, float widthIn, float heightIn, 
        FrameBufferAnalyzer frameBufferAnalyzer, float numMsIn, float numMsOffsetIn )
    {


    }
}



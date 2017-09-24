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


    public ClientDebugPanel()
    {
        showNetPerfPanel = false;
        showLivePingFPSOverlay = false;
        showMiniNetworkMeter = false;

        m_texture2DBackground = PrivateCreateTexture2DColor( 2, 2, 0.5f, 0.5f, 0.5f, 0.9f ); // grey (semi-transparent)
    }

    private static Texture2D PrivateCreateTexture2DColor( int widthIn, int heightIn, float redIn, float greenIn, float blueIn, float alphaIn )
    {
        Texture2D texture2D = new Texture2D( widthIn, heightIn );
        PrivateTexture2DFillWithColor( texture2D, redIn, greenIn, blueIn, alphaIn );
        return texture2D;
    }

    private static void PrivateTexture2DFillWithColor( Texture2D texture2dIn, float redIn, float greenIn, float blueIn, float alphaIn )
    {
        if( texture2dIn == null )
        {
            return;
        }
        Color color = new Color( redIn, greenIn, blueIn, alphaIn );
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

        if (showNetPerfPanel == true)
        {
            RenderNetPerfPanel();
            AddText(0, 0, 200, 200, Color.white, "Nice", TextAnchor.UpperLeft, 32);
        }
        else
        {
        
        }
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

        // row 1
        AddText( x, y, w, dy, Color.white, "Lag Simulator", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddButton( x, y, w, dy, Color.white, "Hiccup", this, new ButtonClickedCallback(delegate( object voidObjectDataIn)
            {
                ClientDebugPanel clientDebugPanel = voidObjectDataIn as ClientDebugPanel;
                if (clientDebugPanel != null)
                {
                    // hiccup
                    Debug.LogError("Implementing Hiccup");
                }
            }));
                
        x = 0;
        y += (dy + gy);

        // row 2
        AddText( x, y, w, dy, Color.white, "Ping", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to fill up Ping]");

        x = 0;
        y += (dy + gy);



        // row 3
        AddText( x, y, w, dy, Color.white, "Receive Clump Quanta", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to put an EDITOR BOX here]");

        x = 0;
        y += (dy + gy);


        // row 4
        AddText( x, y, w, dy, Color.white, "Computed SW", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to put Computed SW here]");

        x = 0;
        y += (dy + gy);

        // row 5: empty line
        y += (dy + gy);

        // row 6
        AddText( x, y, w, dy, Color.white, "Head", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to put Frame Head here]");
        x = 0;
        y += (dy + gy);

        // row 7
        AddText( x, y, w, dy, Color.white, "Tail", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to put Frame Tail here]");
        x = 0;
        y += (dy + gy);


        // row 8
        AddText( x, y, w, dy, Color.white, "Diff + ConsumerCounter", TextAnchor.UpperLeft, 24 );
        x += panelWidth - w;
        AddText(x, y, w, dy, Color.white, "[NEED to put Diff + ConsumerCounter here]");
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

    private void AddSmoothingWindowAnalyserGraph( float ulxIn, float ulyIn, float widthIn, float heightIn, 
        SmoothingWindowAnalyzer smoothingWindowAnalyser, float numMsIn, float numMsOffsetIn )
    {


    }
}



using System;


public class NetMeterManager
{
    private Int64 m_captureWindowSizeMS;
    private float m_numVerticalBytesInMeter;

    // the nearest MS value that we will quantize to
    private float m_quantizeToNearestMS;
    public NetMeterManager()
    {

    }

    public void Init()
    {
        // we will capture 3 seconds worth of data
        SetCaptureWindowSizeMs(3000);
        SetNumVerticalBytesInMeter(50);
    }

    public void SetCaptureWindowSizeMs(Int64 captureWindowSizeMS)
    {
        m_captureWindowSizeMS = captureWindowSizeMS;
    }

    public void SetNumVerticalBytesInMeter(float numVerticalBytesInMeter)
    {
        m_numVerticalBytesInMeter = numVerticalBytesInMeter;
    }

    public Int64 GetCaptureWindowSizeMS()
    {
        return m_captureWindowSizeMS;
    }

    public void Pump()
    {

    }
}


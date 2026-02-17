using System.Collections.Generic;

public static class ErrorHelper
{
    private static readonly Dictionary<int, string> ErrorMessages = new Dictionary<int, string>
    {
        { 0, "Fingerprint captured successfully." },

        // Device / Driver issues
        { 1, "Device could not start. Please restart the app or check installation." },
        { 2, "Device is not working correctly. Reinstall the fingerprint software." },
        { 5, "System file missing. Please reinstall the software." },
        { 6, "Device not connected or driver not loaded. Check USB cable or reinstall driver." },
        { 55, "No fingerprint device connected. Please check USB cable." },
        { 56, "Driver failed to load. Please reinstall driver." },
        { 61, "This fingerprint reader is not supported." },

        // Capture / Image issues
        { 3, "System error occurred. Please try again." },
        { 7, "Fingerprint algorithm failed. Contact support." },
        { 51, "System files missing. Please reinstall the software." },
        { 52, "Fingerprint sensor not responding. Reconnect the device." },
        { 53, "Device connection lost. Please reconnect the device." },
        { 54, "No finger detected in time. Please place your finger again." },
        { 57, "Place your finger flat and properly on the scanner." },
        { 58, "USB bandwidth issue. Try using another USB port." },
        { 59, "Device is busy. Try again in a few seconds." },
        { 60, "Device serial number not readable. Reconnect the device." },

        // Template / Match issues
        { 101, "Fingerprint quality too low. Place finger properly." },
        { 102, "Fingerprint format mismatch. Try again." },
        { 103, "Saved fingerprint data is corrupted." },
        { 104, "Fingerprint template is not valid." },
        { 105, "Fingerprint not clear. Clean the sensor and try again." },
        { 106, "Fingerprint did not match. Try again." },

        // System / License issues
        { 1000, "System is out of memory. Close other apps and retry." },
        { 2000, "Unexpected system error occurred." },
        { 3000, "Unexpected error. Please contact support." },
        { 4000, "System error occurred. Please restart and try again." },
        { 6000, "Security certificate error. Contact support." },
        { 10001, "Fingerprint license validation failed." },
        { 10002, "This license is not valid for your domain." },
        { 10003, "License check failed. Contact support." },
        { 10004, "License expired. Please renew your license." }
    };

    public static (int Code, string Message) GetError(int code)
    {
        if (ErrorMessages.TryGetValue(code, out var message))
            return (code, message);

        return (code, "Unknown error occurred. Please try again.");
    }
}

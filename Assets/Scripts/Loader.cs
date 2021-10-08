using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour
{
    public static Loader L;

    private void Awake()
    {
        L = this;
    }

    public void Load()
    {
        StartCoroutine(CSVDownloader.CSVD.DownloadData(AfterDownload));
    }

    public void AfterDownload(string data)
    {
        if (data == null)
            ManagerUI.MUI.ShowError(true, "Connection Error");
        else
            StartCoroutine(ProcessData(data, AfterProcessData));
    }

    private void AfterProcessData(string errorMessage)
    {
        if (null != errorMessage)
            ManagerUI.MUI.ShowError(true, "Processing Error: " + errorMessage);
        else
        {
            Debug.Log("Download Success");
            ManagerUI.MUI.OpenData();
        }
    }

    public IEnumerator ProcessData(string data, System.Action<string> onCompleted)
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();

        // Line level
        int currLineIndex = 0;
        bool inQuote = false;
        int linesSinceUpdate = 0;
        int kLinesBetweenUpdate = 15;

        // Entry level
        string currEntry = "";
        int currCharIndex = 0;
        bool currEntryContainedQuote = false;
        List<string> currLineEntries = new List<string>();

        // "\r\n" means end of line and should be only occurence of '\r' (unless on macOS/iOS in which case lines ends with just \n)
        char lineEnding = Application.platform == RuntimePlatform.IPhonePlayer ? '\n' : '\r';
        int lineEndingLength = Application.platform == RuntimePlatform.IPhonePlayer ? 1 : 2;

        while (currCharIndex < data.Length)
        {
            if (!inQuote && (data[currCharIndex] == lineEnding))
            {
                // Skip the line ending
                currCharIndex += lineEndingLength;

                // Wrap up the last entry
                // If we were in a quote, trim bordering quotation marks
                if (currEntryContainedQuote)
                {
                    currEntry = currEntry.Substring(1, currEntry.Length - 2);
                }

                currLineEntries.Add(currEntry);
                currEntry = "";
                currEntryContainedQuote = false;

                // Line ended
                ProcessSOTG.PS.ProcessLineFromCSV(currLineEntries, currLineIndex);
                currLineIndex++;
                currLineEntries = new List<string>();

                linesSinceUpdate++;
                if (linesSinceUpdate > kLinesBetweenUpdate)
                {
                    linesSinceUpdate = 0;
                    yield return new WaitForEndOfFrame();
                }
            }
            else
            {
                if (data[currCharIndex] == '"')
                {
                    inQuote = !inQuote;
                    currEntryContainedQuote = true;
                }

                // Entry level stuff
                {
                    if (data[currCharIndex] == ',')
                    {
                        if (inQuote)
                        {
                            currEntry += data[currCharIndex];
                        }
                        else
                        {
                            // If we were in a quote, trim bordering quotation marks
                            if (currEntryContainedQuote)
                            {
                                currEntry = currEntry.Substring(1, currEntry.Length - 2);
                            }

                            currLineEntries.Add(currEntry);
                            currEntry = "";
                            currEntryContainedQuote = false;
                        }
                    }
                    else
                    {
                        currEntry += data[currCharIndex];
                    }
                }
                currCharIndex++;
            }

            ManagerUI.MUI.UpdateProgress(currCharIndex / data.Length);
        }

        //Process last line
        currCharIndex += lineEndingLength;
        if (currEntryContainedQuote)
            currEntry = currEntry.Substring(1, currEntry.Length - 2);
        currLineEntries.Add(currEntry);
        currEntry = "";
        currEntryContainedQuote = false;
        ProcessSOTG.PS.ProcessLineFromCSV(currLineEntries, currLineIndex);

        onCompleted(null);
    }
}
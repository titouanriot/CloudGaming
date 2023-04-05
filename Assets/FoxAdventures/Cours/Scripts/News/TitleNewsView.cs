using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TitleNewsViewEntry
{
    public string title;
    public string body;
    public DateTime displayedDate;

    public string DisplayedDateStr
    {
        get
        {
            return this.displayedDate.ToLongDateString();
        }
    }
}

public class TitleNewsView : MonoBehaviour
{
    // Content root
    public Transform contentRoot = null;

    // Content UI
    public Text contentText = null;

    // Show on
    public bool automaticShowLatestNews = false;

    void OnEnable()
    {
        // Hide by default
        this.HideView();

        // Show latest news
        if (this.automaticShowLatestNews == true)
            this.ShowLatestNews();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) == true)
        {
            this.HideView();
        }
    }

    public void ShowLatestNews()
    {
        Debug.Log("coucou");
        // Trigger news show if logged in
        if (PlayfabAuth.IsLoggedIn == true)
        {
            // TODO: Request playfab to retrieve the latest news
            PlayFabClientAPI.GetTitleNews(new GetTitleNewsRequest(), result => {
                

                this.OnGetTitleNewsSuccess(result.News);
            }, error => Debug.LogError(error.GenerateErrorReport()));
        }
    }

    private void OnGetTitleNewsSuccess(List<TitleNewsItem> newsPlayFab)
    {
        // Fill data of the list
        List<TitleNewsViewEntry> titleNewsViewEntries = newsPlayFab.Select(tni => new TitleNewsViewEntry { title = tni.Title, body = tni.Body, displayedDate = tni.Timestamp }).ToList();
        List<TitleNewsViewEntry> news;
        if (titleNewsViewEntries.Count > 0)
        {
            news = titleNewsViewEntries;
            Debug.Log("entrée if >0");
        }
        else
        {

            news = new List<TitleNewsViewEntry>();
        }

        // News found
        if (news != null && news.Count > 0)
        {
            // Update Content
            if (this.contentText != null)
            {
                string newsContent = string.Empty;
                for (int i = 0; i < news.Count; i++)
                {
                    if (string.IsNullOrWhiteSpace(newsContent) == false)
                        newsContent += "\n\n";

                    // Fill content with our news
                    newsContent += "- " + news[i].DisplayedDateStr + " -";
                    newsContent += "\n<color=orange>" + news[i].title + "</color>";
                    newsContent += "\n" + news[i].body;
                }

                // Update view
                this.contentText.text = newsContent;
            }

            // Show
            this.ShowView();
        }
        // No news
        else
        {
            Debug.Log("no news");
            // Hide view immediately
            this.HideView();
        }
    }

    private void OnGetTitleNewsError()
    {
        // Log
        Debug.LogError("TitleNewsView.OnGetTitleNewsError() - Error: TODO");

        // Hide view immediately
        this.HideView();
    }


    // Show / Hide content root
    public void ShowView()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(true);
    }

    public void HideView()
    {
        if (this.contentRoot != null)
            this.contentRoot.gameObject.SetActive(false);
    }
}

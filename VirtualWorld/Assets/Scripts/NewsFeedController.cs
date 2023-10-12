using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using System.Linq;
using FishNet.Connection;

public class NewsFeedController : NetworkBehaviour
{
    public static NewsFeedController Instance;

    public List<NewsFeedItem> LocalNews;

    public int RunningFreeID;

    public List<NewsFeedItem> GlobalNews;

    public int RunningFreeGlobalID;

    public delegate void NewsUpdated();
    public NewsUpdated OnNewsUpdated;

    // Start is called before the first frame update
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Launched newsfeed controller");

            Instance = this;

            LocalNews = GenerateLocalNewsTestFunction();
        }

    }


    public override void OnStartClient()
    {
        RequestGlobalNewsServerRpc();
        //Debug.Log("Player spawned, should update global news at startup. Probably the call is in the wrong place at Additive Scene Launcher");
    }

    public List<NewsFeedItem> GetLocalNews()
    {
        return LocalNews;
    }


    public List<NewsFeedItem> GetGlobalNews()
    {
        return GlobalNews;
    }



    public List<NewsFeedItem> GenerateLocalNewsTestFunction()
    {
        List<NewsFeedItem> items = new List<NewsFeedItem>();

        for (int i = 0; i < 12; i++)
        {
            items.Add(new NewsFeedItem());
            //items[i].Header = "This is news item number " + i.ToString();

        }

        for (int i = 0; i < items.Count; i++)
        {
            items[i].ID = i;
            RunningFreeID = i + 1;
            //Debug.LogError("Iterating through news items. i is " + i.ToString());
        }

        items[0].Header = "Something has happened. Click for more info!";
        items[1].Header = "A man bit a dog";
        items[2].Header = "Stocks are on the rise";
        items[3].Header = "A Catastrophe has occured";
        items[4].Header = "Farm is producing a record amount of produce";
        items[5].Header = "A politician is caught in a scandal";
        items[6].Header = "Just another headline for testing purposes";
        items[7].Header = "Good news! World peace is finally here.";
        items[8].Header = "Yada yada yada and yada yada";
        items[9].Header = "Have you always done this wrong?";
        items[10].Header = "A programmer made some spaghetti with meatballs";
        items[11].Header = "A programmer was off by one: Married twice.";

        items[0].Content = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
        items[1].Content = "Luckily it was a hot dog. Sed ut perspiciatis unde omnis iste natus error sit voluptatem accusantium doloremque laudantium, totam rem aperiam, eaque ipsa quae ab illo inventore veritatis et quasi architecto beatae vitae dicta sunt explicabo. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit, sed quia consequuntur magni dolores eos qui ratione voluptatem sequi nesciunt. Neque porro quisquam est, qui dolorem ipsum quia dolor sit amet, consectetur";
        items[2].Content = "adipisci velit, sed quia non numquam eius modi tempora incidunt ut labore et dolore magnam aliquam quaerat voluptatem. Ut enim ad minima veniam, quis nostrum exercitationem ullam corporis suscipit laboriosam, nisi ut aliquid ex ea commodi consequatur? Quis autem vel eum iure reprehenderit qui in ea voluptate velit esse quam nihil molestiae consequatur, vel illum qui dolorem eum fugiat quo voluptas nulla pariatur?";
        items[3].Content = "At vero eos et accusamus et iusto odio dignissimos ducimus qui blanditiis praesentium voluptatum deleniti atque corrupti quos dolores et quas molestias excepturi sint occaecati cupiditate non provident, similique sunt in culpa qui officia deserunt mollitia animi, id est laborum et dolorum fuga. ";
        items[4].Content = "Et harum quidem rerum facilis est et expedita distinctio. Nam libero tempore, cum soluta nobis est eligendi optio cumque nihil impedit quo minus id quod maxime placeat facere possimus, omnis voluptas assumenda est, omnis dolor repellendus. Temporibus autem quibusdam et ";
        items[5].Content = "aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae. Itaque earum rerum hic tenetur a sapiente delectus, ut aut reiciendis voluptatibus maiores alias consequatur aut perferendis doloribus asperiores repellat.";
        items[6].Content = "On the other hand, we denounce with righteous indignation and dislike men who are so beguiled and demoralized by the charms of pleasure of the moment, so blinded by desire, that they cannot foresee the pain and trouble that are bound to ensue; and equal blame belongs to those who fail in their duty";
        items[7].Content = "through weakness of will, which is the same as saying through shrinking from toil and pain. These cases are perfectly simple and easy to distinguish. In a free hour, when our power of choice is untrammelled and when nothing prevents our being able to do what we like best, every pleasure";
        items[8].Content = "is to be welcomed and every pain avoided. But in certain circumstances and owing to the claims of duty or the obligations of business it will frequently occur that pleasures have to be repudiated and annoyances accepted. The wise man therefore always holds in these matters to this principle of selection: he rejects pleasures to secure other greater pleasures, or else he endures pains to avoid worse pains";
        items[9].Content = "But I must explain to you how all this mistaken idea of denouncing pleasure and praising pain was born and I will give you a complete account of the system, and expound the actual teachings of the great explorer of the truth, the master-builder of human happiness. No one rejects, dislikes";
        items[10].Content = "avoids pleasure itself, because it is pleasure, but because those who do not know how to pursue pleasure rationally encounter consequences that are extremely painful. Nor again is there anyone who loves or pursues or desires to obtain pain of itself, because it is pain, but because occasionally circumstances occur";
        items[11].Content = "in which toil and pain can procure him some great pleasure. To take a trivial example, which of us ever undertakes laborious physical exercise, except to obtain some advantage from it? But who has any right to find fault with a man who chooses to enjoy a pleasure that has no annoying consequences, or one who avoids a pain that produces no resultant pleasure?";

        items[0].Priority = -1;
        items[1].Priority = 6;
        items[2].Priority = 1;
        items[3].Priority = 2;
        items[4].Priority = 100;
        items[5].Priority = 4;
        items[6].Priority = 8;
        items[7].Priority = -5;
        items[8].Priority = 366;
        items[9].Priority = 3;
        items[10].Priority = 4;
        items[11].Priority = 2;

        items = items.OrderBy(x => x.Priority).ToList();


        //for (int i = 0; i< items.Count; i++)
        //{
        //    Debug.LogError(" Header is " + items[i].Header);
        //}

        return items;
    }

    public NewsFeedItem GetLocalNewsItemByID(int id)
    {
        for (int i = 0; i < LocalNews.Count; i++)
        {
            if (LocalNews[i].ID == id)
            {
                return LocalNews[i];
            }
        }

        return null;
    }

    // NOTE TODO: if we have some specialized ID, we are going to replace the news item
    // This is for the purposes of if we want to track some individual newsworthy
    // subject that changes all the time, we only get the latest news


    public void AddNewsItemToLocalNews(NewsFeedItem item)
    {
        bool foundWithId = false;

        for (int i = 0; i < LocalNews.Count; i++)
        {
            if (LocalNews[i].ID == item.ID)
            {
                LocalNews.Remove(LocalNews[i]);
                foundWithId = true;
            }
        }

        if (!foundWithId)
        {
            item.ID = RunningFreeID;
            RunningFreeID++;

            if (RunningFreeID >= 1000)
            {
                RunningFreeID = 0;
            }
        }

        LocalNews.Add(item);
        LocalNews = LocalNews.OrderBy(x => x.Priority).ToList();

        if (OnNewsUpdated != null)
        {
            OnNewsUpdated();
        }

        //Debug.Log("Adding news item to local news " + Time.time);
    }


    // Probaly sending long strings is not a good thing, but then again websites do that too.
    [ServerRpc(RequireOwnership = false)]
    public void AddNewsItemToGlobalNewsServerRpc(int priority,
                                                 int id,
                                                 string header,
                                                 string content)
    {
        //Debug.Log("Server rpc is called");

        NewsFeedItem item = new NewsFeedItem();
        item.Priority = priority;

        if (id >= 0) 
        {
            item.ID = id;
        }

        else
        {
            item.ID = RunningFreeGlobalID;
            RunningFreeGlobalID++;

            if (RunningFreeGlobalID >= 1000)
            {
                RunningFreeGlobalID = 0;
            }
        }

        item.Header = header;
        item.Content = content;

        for (int i = 0; i < GlobalNews.Count; i++)
        {
            if (GlobalNews[i].ID == item.ID)
            {
                GlobalNews.Remove(GlobalNews[i]);
            }
        }

        GlobalNews.Add(item);

        //Debug.Log("Got through. about to call client rpc");

        UpdateGlobalNewsItemClientRpc(item.Priority,
                                      item.ID,
                                      item.Header,
                                      item.Content);
    }

    [ObserversRpc]
    public void UpdateGlobalNewsItemClientRpc(int priority,
                                              int id,
                                              string header,
                                              string content)
    {
        //Debug.Log("Observers rpc is called");

        for (int i = 0; i < GlobalNews.Count; i++)
        {
            if (GlobalNews[i].ID == id)
            {
                GlobalNews.Remove(GlobalNews[i]);
            }
        }

        NewsFeedItem item = new NewsFeedItem();
        item.Priority = priority;
        item.ID = id;
        item.Header = header;
        item.Content = content;
        GlobalNews.Add(item);
        //Debug.Log("Updating client global news " + item.Header);

        if (OnNewsUpdated != null)
        {
            OnNewsUpdated();
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void RequestGlobalNewsServerRpc()
    {
        //Debug.Log("Requesting global news");

        for (int i = 0; i < GlobalNews.Count; i++)
        {
            UpdateGlobalNewsItemClientRpc(GlobalNews[i].Priority,
                                          GlobalNews[i].ID,
                                          GlobalNews[i].Header,
                                          GlobalNews[i].Content);

            //Debug.Log("Updating item " + GlobalNews[i].Header);
        }
    }



}

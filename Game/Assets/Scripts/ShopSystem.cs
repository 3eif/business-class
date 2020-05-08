using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSystem : MonoBehaviour {

    public Shop miniPlane;
    public Shop jet;
    public Shop commercialPlane;
    public Shop spacecraft;
    public Shop paperPlane;

    private Text planeName;
    private Text planeDescription;
    private Text fuelText;
    private Slider fuelSlider;
    private Button statusButton;
    private Text statusText;
    private Text coinsTxt;

    public GameObject winPanel;

    private GameObject miniPlaneObj;
    public GameObject jetObj;
    private GameObject commercialPlaneObj;
    private GameObject spacecraftObj;
    private GameObject paperPlaneObj;

    public static string[] planes = {"mini plane", "jet", "commercial plane", "spacecraft", "paper plane"};
    private int currIndex = 0;

    public static int selectedIndex;

    private void Awake(){

        planeName = GameObject.Find("/canvas/info panel/title").GetComponent<Text>();
        fuelSlider = GameObject.Find("/canvas/info panel/fuel/fuelSlider").GetComponent<Slider>();
        fuelText = GameObject.Find("/canvas/info panel/fuel/fuelTxt").GetComponent<Text>();
        planeDescription = GameObject.Find("/canvas/info panel/descriptionTxt").GetComponent<Text>();
        statusText = GameObject.Find("/canvas/status button/text").GetComponent<Text>();
        statusButton = GameObject.Find("/canvas/status button").GetComponent<Button>();
        winPanel = GameObject.Find("/canvas/win panel");

        miniPlaneObj = GameObject.Find("/plane models/mini plane");
        jetObj = GameObject.Find("/plane models/jet");
        commercialPlaneObj = GameObject.Find("/plane models/commercial plane");
        spacecraftObj = GameObject.Find("/plane models/spacecraft");
        paperPlaneObj = GameObject.Find("/plane models/paper plane");
        coinsTxt = GameObject.Find("/canvas/hud/coinsPanel/coinsTxt").GetComponent<Text>();

        miniPlaneObj.SetActive(false);
        jetObj.SetActive(false);
        commercialPlaneObj.SetActive(false);
        spacecraftObj.SetActive(false);
        paperPlaneObj.SetActive(false);
        winPanel.SetActive(false);
    }

    private void Start() {

        loadShop();
    }

    // move one plane to the right in the shop
    public void Right(){
        if(currIndex < planes.Length-1) currIndex++;
        loadShop();
    }

    // move one plane to the left in the shop
    public void Left(){
        if(currIndex > 0) currIndex--;
        loadShop();
    }

    public void Back(){
        winPanel.SetActive(false);
    }

    private void loadShop(){
        miniPlaneObj.SetActive(false);
        jetObj.SetActive(false);
        paperPlaneObj.SetActive(false);
        spacecraftObj.SetActive(false);
        commercialPlaneObj.SetActive(false);

        // gets shop status ("Buy", "Select", "Selected") of current plane
        statusText.text = PlayerPrefs.GetString(planes[currIndex], "Select");
        statusButton.interactable = true;

        // display info for mini plane 
        if(planes[currIndex] == planes[0]){
            planeName.text = miniPlane.name;
            planeDescription.text = miniPlane.description;
            fuelText.text = "Fuel: " + miniPlane.fuel.ToString() + "/250";
            fuelSlider.value = miniPlane.fuel/250;
            miniPlaneObj.SetActive(true);
            string status = PlayerPrefs.GetString("mini plane", "Selected");
        }

        // display info for jet
        if(planes[currIndex] == planes[1]){
            planeName.text = jet.name;
            planeDescription.text = jet.description;
            fuelText.text = "Fuel: " + jet.fuel.ToString() + "/250";
            fuelSlider.value = jet.fuel/250;
            jetObj.SetActive(true);

            if(PlayerPrefs.GetString(planes[currIndex], "Buy") == "Buy"){
                statusText.text = "Buy" + " (" + jet.price + " Jewels)";
                int coinsNum = PlayerPrefs.GetInt("coins", 0);
                coinsNum -= jet.price;
                if(coinsNum < 0) {
                    statusButton.interactable = false;
                } else {
                    statusButton.interactable = true;
                }
            } else {
                statusButton.interactable = true;
            }
        }

        // display info for commercial plane
        if(planes[currIndex] == planes[2]){
            planeName.text = commercialPlane.name;
            planeDescription.text = commercialPlane.description;
            fuelText.text = "Fuel: " + commercialPlane.fuel.ToString() + "/250";
            fuelSlider.value = commercialPlane.fuel/250;
            commercialPlaneObj.SetActive(true);

            if(PlayerPrefs.GetString(planes[currIndex], "Buy") == "Buy"){
                statusText.text = "Buy" + " (" + commercialPlane.price + " Jewels)";
                int coinsNum = PlayerPrefs.GetInt("coins", 0);
                coinsNum -= commercialPlane.price;
                if(coinsNum < 0) {
                    statusButton.interactable = false;
                } else {
                    statusButton.interactable = true;
                }
            } else {
                statusButton.interactable = true;
            }
        }

        // display info for spacecraft
        if(planes[currIndex] == planes[3]){
            planeName.text = spacecraft.name;
            planeDescription.text = spacecraft.description;
            fuelText.text = "Fuel: " + spacecraft.fuel.ToString() + "/250";
            fuelSlider.value = spacecraft.fuel/250;
            spacecraftObj.SetActive(true);

            if(PlayerPrefs.GetString(planes[currIndex], "Buy") == "Buy"){
                statusText.text = "Buy" + " (" + spacecraft.price + " Jewels)";
                int coinsNum = PlayerPrefs.GetInt("coins", 0);
                coinsNum -= spacecraft.price;
                if(coinsNum < 0) {
                    statusButton.interactable = false;
                } else {
                    statusButton.interactable = true;
                }
            } else {
                statusButton.interactable = true;
            }
        }

        // display info for paper plane
        if(planes[currIndex] == planes[4]){
            planeName.text = paperPlane.name;
            planeDescription.text = paperPlane.description;
            fuelText.text = "Fuel: infinite";
            fuelSlider.value = 1;
            paperPlaneObj.SetActive(true);

            // validates whether the user has enough coins. If not, they cannot click the buy button

            if(PlayerPrefs.GetString(planes[currIndex], "Buy") == "Buy"){
                statusText.text = "Buy" + " (" + paperPlane.price + " Jewels)";
                int coinsNum = PlayerPrefs.GetInt("coins", 0);
                coinsNum -= paperPlane.price;

                if (coinsNum < 0) {
                    statusButton.interactable = false;
                } else {
                    statusButton.interactable = true;
                }
            } else {
                statusButton.interactable = true;
            }
        }
    }

    public void PlaneOption(){
        if(PlayerPrefs.GetString(planes[currIndex], "Buy") == "Buy"){

            // the player buys a plane

            statusText.text = "Select";
            statusButton.interactable = true;
            PlayerPrefs.SetString(planes[currIndex], "Select");

            // subtracts coins from the player's bank

            int coinsNum = PlayerPrefs.GetInt("coins", 0);
            if(planes[currIndex] == planes[0])  coinsNum -= miniPlane.price;
            if(planes[currIndex] == planes[1])  coinsNum -= jet.price;
            if(planes[currIndex] == planes[2])  coinsNum -= commercialPlane.price;
            if(planes[currIndex] == planes[3])  coinsNum -= spacecraft.price;
            if(planes[currIndex] == planes[4])  {coinsNum -= paperPlane.price; winPanel.SetActive(true);}
            PlayerPrefs.SetInt("coins", coinsNum);
            PlayerPrefs.Save();
            coinsTxt.text = PlayerPrefs.GetInt("coins", 0).ToString();
        } else {

            // the player selects a plane

            if(PlayerPrefs.GetString(planes[currIndex], "Select") == "Select"){
                statusText.text = "Selected";
                selectedIndex = currIndex;

                // the player selects mini plane
                if(planes[currIndex] == planes[0]){
                    if(PlayerPrefs.GetString("paper plane", "Buy") != "Buy") PlayerPrefs.SetString("paper plane", "Select");
                    if(PlayerPrefs.GetString("spacecraft", "Buy") != "Buy") PlayerPrefs.SetString("spacecraft", "Select");
                    if(PlayerPrefs.GetString("jet", "Buy") != "Buy") PlayerPrefs.SetString("jet", "Select");
                    if(PlayerPrefs.GetString("commercial plane", "Buy") != "Buy") PlayerPrefs.SetString("commercial plane", "Select");
                    PlayerPrefs.SetString("mini plane", "Selected");
                }

                // the player selects jet
                if(planes[currIndex] == planes[1]){
                    if(PlayerPrefs.GetString("paper plane", "Buy") != "Buy") PlayerPrefs.SetString("paper plane", "Select");
                    if(PlayerPrefs.GetString("spacecraft", "Buy") != "Buy") PlayerPrefs.SetString("spacecraft", "Select");
                    if(PlayerPrefs.GetString("commercial plane", "Buy") != "Buy") PlayerPrefs.SetString("commercial plane", "Select");
                    PlayerPrefs.SetString("jet", "Selected");
                    PlayerPrefs.SetString("mini plane", "Select");
                }

                // the player selects commercial plane
                if(planes[currIndex] == planes[2]){
                    if(PlayerPrefs.GetString("paper plane", "Buy") != "Buy") PlayerPrefs.SetString("paper plane", "Select");
                    if(PlayerPrefs.GetString("spacecraft", "Buy") != "Buy") PlayerPrefs.SetString("spacecraft", "Select");
                    if(PlayerPrefs.GetString("jet", "Buy") != "Buy") PlayerPrefs.SetString("jet", "Select");
                    PlayerPrefs.SetString("commercial plane", "Selected");
                    PlayerPrefs.SetString("mini plane", "Select");
                }

                // the player selects spacecraft
                if(planes[currIndex] == planes[3]){
                    if(PlayerPrefs.GetString("paper plane", "Buy") != "Buy") PlayerPrefs.SetString("paper plane", "Select");
                    if(PlayerPrefs.GetString("jet", "Buy") != "Buy") PlayerPrefs.SetString("jet", "Select");
                    if(PlayerPrefs.GetString("commercial plane", "Buy") != "Buy") PlayerPrefs.SetString("commercial plane", "Select");
                    PlayerPrefs.SetString("spacecraft", "Selected");
                    PlayerPrefs.SetString("mini plane", "Select");
                }

                // the player selects paper plane
                if(planes[currIndex] == planes[4]){
                    if(PlayerPrefs.GetString("commercial plane", "Buy") != "Buy") PlayerPrefs.SetString("commercial plane", "Select");
                    if(PlayerPrefs.GetString("spacecraft", "Buy") != "Buy") PlayerPrefs.SetString("spacecraft", "Select");
                    if(PlayerPrefs.GetString("jet", "Buy") != "Buy") PlayerPrefs.SetString("jet", "Select");
                    PlayerPrefs.SetString("paper plane", "Selected");
                    PlayerPrefs.SetString("mini plane", "Select");
                }
            }

        }
    }
}


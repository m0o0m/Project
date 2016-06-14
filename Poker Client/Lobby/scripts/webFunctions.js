/**
 * Created by mmdamin on 4/11/2016.
 */
function showMsgBox(name,msg)
{
    var Box = document.createElement("div");
    Box.setAttribute("class","msgbox");

    var BlackBackScreen = document.createElement("div");
    BlackBackScreen.setAttribute("class","BlackBackScreen");

    var header = document.createElement("div");
    header.setAttribute("class","msgbox-header");
    var innerHeader = document.createElement("div");
    innerHeader.setAttribute("class","innerHeader");
    innerHeader.innerHTML ='<a style="margin: 0 auto">'+name+"</a>";
    header.appendChild(innerHeader);
    Box.appendChild(header);

    var text = document.createElement("div");
    text.setAttribute("class","msgbox-text");
    text.innerHTML = msg;
    Box.appendChild(text);

    var btn = document.createElement("btn");
    btn.setAttribute("class","msgbox-btn");
    btn.addEventListener("click",function(){
        BlackBackScreen.parentNode.removeChild(BlackBackScreen);
        Box.parentNode.removeChild(Box);
    });

    Box.appendChild(btn);

    BlackBackScreen.addEventListener("click",function(){
        BlackBackScreen.parentNode.removeChild(BlackBackScreen);
        Box.parentNode.removeChild(Box);
    });

    document.getElementById("main-container").appendChild(BlackBackScreen);
    document.getElementById("main-container").appendChild(Box);
}
function addTable(tableName,tableId,seatsCount,MaxBuyin,MinBuyin,BigBlind,PlayersCount){
    if(document.getElementById(tableId) != null)
        return;
    var container = document.getElementById("infobox-container");
    if(container == null)
        return;

    var tableHandle = document.createElement("div");
    tableHandle.addEventListener("click",function(){
        $(".infobox-details").slideUp();
        $("#"+tableId+"-details").slideToggle();
    });
    tableHandle.setAttribute("id",tableId);
    tableHandle.setAttribute("class","cashtable-infobox");

    var TableName = document.createElement("div");
    TableName.setAttribute("class","tablehandle-info");
    TableName.setAttribute("id","tablename-" + tableId);
    TableName.innerHTML = "<a>" + tableName + "</a>";
    tableHandle.appendChild(TableName);

    var tableSeatCount = document.createElement("div");
    tableSeatCount.setAttribute("class","tablehandle-info");
    tableSeatCount.setAttribute("id","tableId-" + tableId);
    tableSeatCount.innerHTML = "<a>" + PlayersCount + "/" + seatsCount + "</a>";
    tableHandle.appendChild(tableSeatCount);

    var Buyin = document.createElement("div");
    Buyin.setAttribute("class","tablehandle-info");
    Buyin.setAttribute("id","Buyin-" + tableId);
    Buyin.innerHTML = "<a>" + MinBuyin + " - " + MaxBuyin + "</a>";
    tableHandle.appendChild(Buyin);

    var Blinds = document.createElement("div");
    Blinds.setAttribute("class","tablehandle-info");
    Blinds.innerHTML = "<a>" + BigBlind/2 + "/" + BigBlind + "</a>";
    Blinds.setAttribute("id","Blinds-" + tableId);
    tableHandle.appendChild(Blinds);
//
    var TableHandleDetails = document.createElement("div");
    TableHandleDetails.setAttribute("id",tableId+"-details");
    TableHandleDetails.setAttribute("class","infobox-details");

    var OpenButton = document.createElement("div");
    OpenButton.setAttribute("class","Button");
    OpenButton.setAttribute("id","Open-"+tableId);
    OpenButton.addEventListener("click", function() {CreateTable(tableName,tableId,BigBlind,seatsCount)} );
    TableHandleDetails.appendChild(OpenButton);

    var DetailsBox = document.createElement("div");
    DetailsBox.setAttribute("class","Details-Box");

    var PlayersName = document.createElement("div");
    PlayersName.setAttribute("class","Detail");
    PlayersName.setAttribute("id",tableId + "-Players-details");

    var Chips = document.createElement("div");
    Chips.setAttribute("class","Detail");
    PlayersName.setAttribute("id",tableId + "-Chips-details");

    var Net = document.createElement("div");
    Net.setAttribute("class","Detail");
    PlayersName.setAttribute("id",tableId + "-Net-details");

    var Waitings = document.createElement("div");
    Waitings.setAttribute("class","Detail");
    PlayersName.setAttribute("id",tableId + "-Waitings-details");

    {
        DetailsBox.appendChild(PlayersName);
        DetailsBox.appendChild(Chips);
        DetailsBox.appendChild(Net);
        DetailsBox.appendChild(Waitings);
    }

    container.appendChild(tableHandle);
    container.appendChild(TableHandleDetails);
}

function SortDetials(tableId,By)
{

}

function SetupViewHandlers()
{
    $("#profile-viewer").show();
    $("#cashgames-viewer").hide();
    $("#tournoments-viewer").hide();
    $("#rollet-viewer").hide();
    $("#sitsandgoes-viewer").hide();
    $("#profile-btn").click(function() {
        $("#profile-viewer").show();
        $("#cashgames-viewer").hide();
        $("#tournoments-viewer").hide();
        $("#rollet-viewer").hide();
        $("#sitsandgoes-viewer").hide();
    });
    $("#cashgames-btn").click(function() {
        $("#profile-viewer").hide();
        $("#cashgames-viewer").show();
        $("#tournoments-viewer").hide();
        $("#rollet-viewer").hide();
        $("#sitsandgoes-viewer").hide();
    });
    $("#tournoments-btn").click(function() {
        $("#profile-viewer").hide();
        $("#cashgames-viewer").hide();
        $("#tournoments-viewer").show();
        $("#rollet-viewer").hide();
        $("#sitsandgoes-viewer").hide();
    });
    $("#sitsandgoes-btn").click(function() {
        $("#profile-viewer").hide();
        $("#cashgames-viewer").hide();
        $("#tournoments-viewer").hide();
        $("#rollet-viewer").hide();
        $("#sitsandgoes-viewer").show();
    });
    $("#rollet-btn").click(function() {
        $("#profile-viewer").hide();
        $("#cashgames-viewer").hide();
        $("#tournoments-viewer").hide();
        $("#rollet-viewer").show();
        $("#sitsandgoes-viewer").hide();
    });

    $("#messages-handle").click(function(){
        $("#messages").show();
        $("#messages-handle").hide();
    });
    $("#messages-close-handle").click(function(){
        $("#messages").hide();
        $("#messages-handle").show();
    });

}

function loading()
{
    var loader = document.getElementById("loader");
    loader.innerHTML = "<h1>Loading Please Wait...</h1>";
    var IntervalId = setInterval(function(){
        if(connected)
        {
            clearInterval(IntervalId);
            loader.parentNode.removeChild(loader);
            fixStyle();
            SetupViewHandlers();
        }
    },2000);
}

var xx;
function LoadProfileImage()
{
    $("#profile-image-viewer").css("background-image","url("+url+"UsersData/"+UserName+"/profile.png?time="+new Date().getTime()+")");
}
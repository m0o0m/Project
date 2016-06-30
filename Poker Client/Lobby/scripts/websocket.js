/**
 * Created by mmdamin on 3/23/2016.
 */
function connect()
{
    if(connected)
        return;
    UserName = document.getElementById("user_id_store").value;
    var session  = document.getElementById("session_id_store").value;
    conn = new WebSocket("ws://" + IpAddress + ":8080");
    conn.onmessage = function(e) {
        messageDispatcher(e.data);
    }
    conn.onclose = function() {
        connected = false;
        showMsgBox("Error!","Connection is Disconnected! <br>Trying to Connect...");
    }
    conn.onerror = function() {
        //showMsgBox("Error!","Connection is Disconnected! <br>Trying to Connect...");
    }
    conn.onopen = function(){
        conn.send(session+";"+UserName);
        connected = true;
    }
}
function sendMessage(type,data){
    var msg = "t="+type+";";
    msg+=data;
    conn.send(msg);
    //each data like it => name=value;
}
function messageDispatcher(msg)
{
    var data = JSON.parse(msg);
    if(data.ms == null || data.ms.type == null) {
        return;
    }
    switch (data.ms.type)
    {
        case "srvmsg":
        {
            if(data.ms.msgtype == null){
                console.log("here");
                return;
            }
            switch (data.ms.msgtype)
            {
                case "tableinfo":
                {
                    addTable(
                        data.ms.tableName,
                        data.ms.tableId,
                        data.ms.seatsCount,
                        data.ms.maxBuyin,
                        data.ms.minBuyin,
                        data.ms.BigBlind,
                        data.ms.PlayersCount);
                    break;
                }
                case "rsv":
                {
                    var pos = data.ms.pos;
                    var username = data.ms.username;
                    var tableid = data.ms.tableId;

                    var seatElem = document.getElementById(tableid+"-seat-"+pos);
                    if(seatElem != null){
                        $(seatElem);
                    }
                    break;
                }
                case "rsvTime":
                {
                    var pos = data.ms.pos;
                    var ExistTime = data.ms.timeExist;
                    var tableId = data.ms.tableId;
                    var timerElem = document.getElementById("rsvTimer-"+tableId);
                    timerElem.innerHTML = "You have "+ ExistTime +" second to buy chips...";

                    break;
                }
                case "rsvShow":
                {
                    var tableId = data.ms.tableId;
                    var pos = data.ms.pos;
                    var minBuyIn = data.ms.maxBuyIn;
                    var maxBuyIn = data.ms.minBuyIn;
                    $("#"+tableId+"-seat-"+pos).css("box-shadow","1px 1px 10px 1px pink");
                    $("#"+tableId+"-seat-"+pos)[0].innerHTML = "<a>reserved</a>";
                    showReserveBox(tableId,pos,minBuyIn,maxBuyIn);
                    break;
                }
                case "ursv":
                {
                    var pos = data.ms.pos;
                    var tableid = data.ms.tableId;
                    var seatElem = document.getElementById(tableid+"-seat-"+pos);
                    if(seatElem != null){
                        $(seatElem);
                    }

                    break;
                }
                case "unrsv":
                {
                    var tableId = data.ms.tableId;
                    var pos = data.ms.pos;
                    setTimeout(function(){
                        var rsvBoxElem = document.getElementById("rsvbox-"+pos+"-"+tableId);
                        console.log(rsvBoxElem);
                        if(rsvBoxElem != null) {
                            console.log("here");
                            rsvBoxElem.parentNode.removeChild(rsvBoxElem);
                            $("#" + tableId + "-seat-" + pos).css("box-shadow", "1px 1px 10px 1px gold");
                            $("#" + tableId + "-seat-" + pos)[0].innerHTML = "<a>sit here</a>";
                        }
                    },100);
                    break;
                }
                case "error":
                {
                  var tableId = data.ms.tableId;
                  var message = data.ms.message;
                  showError(tableId,message);
                }
            }
            break;
        }
        case "act":
        {
            var t = data.ms.actType;
            switch (t)
            {
                case "sitdown":
                {
                    var username = data.ms.username;
                    var chip = data.ms.chip;
                    var position = data.ms.position;
                    var tableId = data.ms.tableId;
                    var amount = data.ms.chip;

                    SitOnSeat(username,tableId,position);
                    setPlayerChip(amount,tableId,position);

                    break;
                }
                case "situp":
                {

                    break;
                }
                case "call":
                {

                    break;
                }
                case "check":
                {

                    break;
                }
                case "fold":
                {
                  alert("fold");
                  break;
                }
                case "bb":
                {

                    break;
                }
                case "sb":
                {

                    break;
                }
            }
            break;
        }
        case "tableData":
        {
            var dataType = data.ms.dataType;
            var tableId = data.ms.tableId;
            switch(dataType)
            {
                case "seatdata":
                {
                  var pos = data.ms.pos;
                  var tableId = data.ms.tableId;

                  var table = OpenTables.find(tableId);
                  if(table!= null){
                    if(data.ms.sitted == "yes")
                    {
                      var playerName = data.ms.playerName;
                      var chip       = data.ms.chip;
                      var timeBank   = data.ms.timeBank;
                      var lastMove   = data.ms.lastMove;

                      table.updateSeatData(playerName,chip,lastMove,timeBank,pos);
                    }
                    else if(data.ms.sitted == "no")
                    {
                      table.SitUpSeat(pos);
                    }
                  }
                }
                case "tableFirstData":
                {
                  var tableId = data.ms.tableId;
                  var tableName = data.ms.tableName;
                  var tableSituation = data.ms.tableSituation;
                  var bigBlind = data.ms.bigBlind;
                  var tablePot = data.ms.tablePot;
                  var flopCards = [data.ms.c1,data.ms.c2,data.ms.c3,data.ms.c4,data.ms.c5];
                  var currentPos = data.ms.currentPos;
                  var dealerPos = data.ms.dealerPos;
                  var seatsCount = data.ms.seatsCount;

                  var table = OpenTables.addTable(tableName,tableId,tableSituation,bigBlind,tablePot,flopCards,currentPos,dealerPos,seatsCount);
                  table.show();
                }
                case "initnewgame":
                {
                    var tableId = data.ms.tableId;
                    var dealerPos = data.ms.dealerPos;
                    setDealerPos(tableId,dealerPos);
                    break;
                }
                case "ucard":
                {
                    var firstCard = data.ms.c1;
                    var secondCard= data.ms.c2;

                    setMyCard(firstCard,secondCard,tableId);
                    break;
                }
                case "flopShow":
                {
                    var cards = [data.ms.c1 ,data.ms.c2 , data.ms.c3];
                    console.log("here");
                    showFlopCards(cards,tableId);
                    break;
                }
                case "turnShow":
                {
                    var ct = data.ms.ct;
                    showTurnCard(ct,tableId);
                    break;
                }
                case "riverShow":
                {
                    var cr = data.ms.cr;
                    showRiver(cr,tableId);
                }
                case "timeExist":
                {
                  var tableId = data.ms.tableId;
                  var time = data.ms.t;
                  var pos = data.ms.pos;
                  updatePlayerTime(tableId,time,pos);
                  break;
                }
                case "timeBank":
                {

                  break;
                }
                case "turnPos":
                {

                  break;
                }
            }

        }

    }
}

function sendOpenTableRequest(tableId)
{
  conn.send("t=opt;"+tableId+";");
}

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
    console.log(conn);
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
                        data.ms.MaxBuyin,
                        data.ms.MinBuyin,
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









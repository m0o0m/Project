/**
 * Created by mmdamin on 4/10/2016.
 */
var tablesData = {};

function addTableData(tableId)
{
    var tableDataIndex;
    for(var i = 0 ; i < tablesData.length ; i++)
    {
        if(tablesData[i].tableId == tableId)
        {
            tableDataIndex = i;
            break;
        }
    }
}
removeTableData(tableId)
{

}
function getTableData(tableId,data)
{

}
function updateTableData(tableId,
                         seatsPlayersUsername,
                         PlayersChips,
                         PlayersSituations)
{

}

function drag(elem,firstX,firstY,firstElemX,firstElemY)
{
    var dragger= function(e){
    $(elem).css("left", firstElemY + (e.clientX - firstX));
    $(elem).css("top",firstElemX + (e.clientY - firstY));
    }

    elem.addEventListener("mouseleave",dragger);
    elem.addEventListener("mousemove",dragger);

    elem.addEventListener("mouseup",function(){
        elem.removeEventListener("mouseleave",dragger);
        elem.removeEventListener("mousemove",dragger);
    });

    $(document)[0].addEventListener("mouseleave",function(){
        elem.removeEventListener("mouseleave",dragger);
        elem.removeEventListener("mousemove",dragger);
    });
}
function CreateTable(tableName,tableId,bb,seatsCount)
{
    //everything of table is on tableBox

    var elem = document.getElementById("table-"+tableId);
    if(elem != null)
    {
        elem.focus();
        return;
    }

    var tableBox = document.createElement("div");
    tableBox.setAttribute("class","tableBox");
    tableBox.setAttribute("id","table-"+tableId);

    var tableHeader = document.createElement("div");
    tableHeader.setAttribute("class","tableHeader");
    tableHeader.innerHTML = tableName + " " + bb/2 + "/" + bb;
    tableHeader.addEventListener("mousedown",function(e) {
        var firstX = e.clientX;
        var firstY = e.clientY;
        var firstElemY = tableBox.offsetLeft;
        var firstElemX = tableBox.offsetTop;
        drag(tableBox,firstX,firstY,firstElemX,firstElemY);
        });
    tableBox.appendChild(tableHeader);

    var tableContainer = document.createElement("div");
    tableContainer.setAttribute("class","tableContainer");

    var table = document.createElement("div");
    table.setAttribute("class","table");

    var flop = document.createElement("div");
    flop.setAttribute("class","flop");
    flop.setAttribute("id",tableId+"-flop");
    table.appendChild(flop);

    var chatBox = document.createElement("div");
    chatBox.setAttribute("id",tableId+"-chatbox");
    chatBox.setAttribute("class","chatBox");
    tableContainer.appendChild(chatBox);


    var dealerBtn = document.createElement("div");
    dealerBtn.setAttribute("class","dealerBtn");
    dealerBtn.setAttribute("id",tableId+"-dealerBtn");
    table.appendChild(dealerBtn);

    tableContainer.appendChild(table);

    //creating seats
    for(var i = 0 ; i < seatsCount ; i++)
    {
        var seat = document.createElement("div");
        seat.setAttribute("class","seat-have-no-player" + " seat"+i);
        seat.setAttribute("id",tableId + "-seat-" + i);
        seat.innerHTML = "<a>sit here</a>";

        tableContainer.appendChild(seat);

        addReserveHandler(seat,tableId,i);
    }
    tableBox.appendChild(tableContainer);
    document.getElementById("container").appendChild(tableBox);

    conn.send("t=opt;"+tableId+";");
}
function addReserveHandler(elem,tableId,pos)
{
    if(elem != null)
    {
        elem.addEventListener("click",function(){
            conn.send("t=rsv;"+tableId+";"+pos);
        });
    }
}
function showReserveBox(tableId,pos,minBuyin,maxBuyin)
{
    var reserveBox = document.createElement("div");
    reserveBox.setAttribute("class", "reserve-box");
    reserveBox.setAttribute("id", "rsvbox-"+pos+"-"+tableId);

    //timer - text - amountbox - cancel - ok
    var header = document.createElement("div");
    header.setAttribute("class","rsvBox-header");
    header.innerHTML = "<a> Buying Chips </a>";
    reserveBox.appendChild(header);

    var text = document.createElement("div");
    text.setAttribute("class","rsvText");
    text.innerHTML = "<a> Please Enter Buy in Amount and start to Playing ";
    text.innerHTML += "<br> Maximum Buyin : " + maxBuyin;
    text.innerHTML += "<br> Minimum Buyin : " + minBuyin;
    text.innerHTMl += "</a>";
    reserveBox.appendChild(text);

    var timer = document.createElement("div");
    timer.setAttribute("class","rsvTimer");
    timer.setAttribute("id","rsvTimer-"+tableId);
    timer.innerHTML += "You have 30 second to buy chips...";
    reserveBox.appendChild(timer);

    var amount = document.createElement("input");
    amount.setAttribute("class","amount-buy-chips");
    amount.setAttribute("id","amount-buy-chips-"+tableId);
    amount.setAttribute("value",((parseInt(maxBuyin)+parseInt(minBuyin))/2));
    amount.setAttribute("type","text");
    reserveBox.appendChild(amount);

    var minCheck = document.createElement("input");
    minCheck.setAttribute("class","max-buy-in-check");
    minCheck.setAttribute("type","checkbox");
    var maxCheck = document.createElement("input");
    maxCheck.setAttribute("class","min-buy-in-check");
    maxCheck.setAttribute("type","checkbox");
    $(maxCheck).on("change",function(){
        amount.value = maxBuyin;
        if(minCheck.checked)
            minCheck.checked = false;
    });
    $(minCheck).on("change",function(){
        amount.value = minBuyin;
        if(maxCheck.checked)
            maxCheck.checked = false;
    });
    reserveBox.appendChild(minCheck);
    reserveBox.appendChild(maxCheck);

    var okBtn = document.createElement("input");
    okBtn.setAttribute("class","ok-rsv-btn");
    okBtn.setAttribute("id","buyin-accept-"+tableId);
    okBtn.setAttribute("type","button");
    okBtn.setAttribute("value","Ok");
    okBtn.addEventListener("click",function(){
        var chips = amount.value;
        conn.send("t=sitd;"+tableId+";"+chips+";"+pos+";");
        if(reserveBox != null)
        {
            reserveBox.parentNode.removeChild(reserveBox);
        }
    });
    reserveBox.appendChild(okBtn);

    var cancel = document.createElement("input");
    cancel.setAttribute("class","cancel-rsv-btn");
    cancel.setAttribute("id","cancel-buyin-"+tableId);
    cancel.setAttribute("type","button");
    cancel.setAttribute("value","Cancel");
    cancel.addEventListener("click",function(){
        conn.send("t=unrsv;"+tableId+";");
    });
    reserveBox.appendChild(cancel);

    var parent = document.getElementById("table-"+tableId);

    if(parent == null)
        return; //error;
    parent.appendChild(reserveBox);
}
function showControl(tableId,elems)
{
    var table = document.getElementById("table-"+tableId);
    var ControlBox = document.createElement("div");
    ControlBox.setAttribute("class","control-box");
    ControlBox.setAttribute("id","ControlBox-"+tableId);

    if(betAble()){
        var bet = document.createElement("div");
    }else {
        var Raise = document.createElement("div");
    }

    if(isCheckable()) {
        var Check = document.createElement("div");
        checkFlag = 1;
    }
    else{
        var Call = document.createElement("div");
    }

    var Fold = document.createElement("div");
    Fold.setAttribute("class","actBtn");
    Fold.innerHTML = "<a>Fold</a>";
    $(Fold).click(function(){
        alert("You Fold..");
    });

    table.appendChild(ControlBox);

}
function deleteControl(tableId)
{
    var ControlBox = document.getElementById("ControlBox-"+tableId);
    if(ControlBox != null)
    {
        ControlBox.parentNode.removeChild(ControlBox);
    }
}
function SitOnSeat(username,tableId,pos)
{
    var seat =  document.getElementById(tableId+"-seat-"+pos);

    if(seat != null) {

        seat.innerHTML = "";//free seat

        var seatHeader = document.createElement("div");
        seatHeader.innerHTML = "<a>" + username + "</a>";
        seatHeader.setAttribute("class", "seatHeader");
        seat.appendChild(seatHeader);

        var seatImage = document.createElement("div");
        seatImage.setAttribute("class", "seatImage");
        $(seatImage).css("background-image","url("+url+"UsersData/"+username+"/profile.png?time="+new Date().getTime()+")");
        seat.appendChild(seatImage);

        var seatChipCount = document.createElement("div");
        seatChipCount.setAttribute("class","seatChipCount");
        seatChipCount.setAttribute("id",tableId+"-seatChipCount-"+pos);

        var seatTimer = document.createElement("div");
        seatTimer.setAttribute("id",tableId+"-seatTimer-"+pos);
        seatTimer.setAttribute("class","seatTimer");
        seat.appendChild(seatTimer);

        var playerChip = document.createElement( "div");
        playerChip.setAttribute("class" , "playerChip");
        playerChip.setAttribute("id"    , tableId+"-playerChip-"+pos);
        seat.appendChild(playerChip);

        var cardHolder = document.createElement("div");
        cardHolder.setAttribute("id","cardholder-"+tableId+"-seat-"+username);
        cardHolder.setAttribute("class","cardHolder");
        seat.appendChild(cardHolder);

        $(seat).removeClass("seat-have-no-player");
        $(seat).addClass("seat-have-player");
    }
}
function SitUpSeat(tableId,pos)
{
    var seat = document.getElementById(tableId+"-seat-"+pos);
    $(seat).innerHTML = "<a>Seat Here</a>";
    addReserveHandler(seat,tableId,pos);
    $(seat).removeClass("seat-have-player");
    $(seat).addClass("seat-have-no-player");
}
function setPlayerChip(amount,tableId,pos)
{
    document.getElementById(tableId+"-playerChip-"+pos).innerHTML = "<a>"+amount+"</a>";
}
function getCard(card1,card2,tableId)
{
}
function setDealerPos(tableId,Pos)
{
    var TopPos = $("#"+tableId+"-seat-"+Pos).offset().top;
    var LeftPos = $("#"+tableId+"-seat-"+Pos).offset().left;

    console.log(TopPos + "  :  " +LeftPos);

    $("#"+tableId+"-dealerBtn").offset({top : TopPos , left : LeftPos});
}
function setMyCard(card1,card2,tableId)
{

    var cardholder = document.getElementById("cardholder-" + tableId + "-seat-" + UserName);

    var firstCard = document.createElement("div");
    var secondCard = document.createElement("div");

    firstCard.setAttribute("class", "card");
    secondCard.setAttribute("class", "card");

    firstCard.setAttribute("id", "card1-" + UserName + "-" + tableId);
    secondCard.setAttribute("id", "card2-" + UserName + "-" + tableId);

    var name = getCardName(card1);
    var path1;
    if(name != false)
        path1 = "../lobby/styles/images/cards/"+ name +".png";
    else
        conn.disconnect();

    name = getCardName(card2);
    var path2;
    if(name != false)
        path2 = "../lobby/styles/images/cards/"+ name +".png";
    else
        conn.disconnect();

    $(firstCard).css("background-image",'url("'+path1+'")');
    $(secondCard).css("background-image",'url("'+path2+'")');

    cardholder.appendChild(firstCard);
    cardholder.appendChild(secondCard);
}
function getCardName(num)
{
    var number = (num%13);
    if(number == 0)
        number = 13;
    var name = number + "_of_";
    if(num >= 1 && num <= 13)
        name+= "spades";
    else if(num >= 14 && num <= 26)
        name += "hearts";
    else if(num >= 27 && num <= 39)
        name += "clubs";
    else if(num >= 40 && num <= 52)
        name += "diamonds";
    else
        return false;
    return name;
}
function showFlopCards(cards,tableId)
{
    console.log(cards);
    var flop = document.getElementById(tableId+"-flop");
    flop.innerHTML = "";
    var card = [ document.createElement("div"),document.createElement("div"),document.createElement("div")];
    var name;
    for(var i = 0 ; i < 3 ; i++)
    {
        name = getCardName(cards[i]);
        card[i].setAttribute("class","flopCard");
        card[i].setAttribute("id",tableId+"-flopcard" + i);
        $(card[i]).css("background-image","url('../lobby/styles/images/cards/"+ name +".png')");
        flop.appendChild(card[i]);
    }

}
function showTurnCard(card,tableId)
{
    var flop = document.getElementById(tableId+"-flop");
    name = getCardName(card);
    turn = document.createElement("div");
    turn.setAttribute("class","flopCard");
    turn.setAttribute("id",tableId+"-flopcard" + i);
    $(turn).css("background-image","../lobby/styles/images/cards/"+ name +".png");
    flop.appendChild(turn);
}
function showRiverCard(card,tableId)
{
    var flop = document.getElementById(tableId+"-flop");
    name = getCardName(card);
    river = document.createElement("div");
    river.setAttribute("class","flopCard");
    river.setAttribute("id",tableId+"-flopcard" + i);
    $(turn).css("background-image","../lobby/styles/images/cards/"+ name +".png");
    flop.appendChild(turn);
}


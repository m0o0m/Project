/**
 * Created by mmdamin on 4/10/2016.
 */
 class SeatData
 {
   constructor(playerName,situation,chip,timeBank,timeExist,lastMove,lastBet)
   {
     this.playerName = playerName;
     this.chip       = chip;
     this.timeBank   = timeBank;
     this.timeExist  = timeExist;
     this.lastMove   = lastMove;
     this.lastBet    = lastBet;
   }
 }
 class Table
 {
   constructor(tableName,tableId,tableSituation,bigBlind,tablePot,flopCards,currentPos,dealerPos,seatsCount) {
     //table data
     this.tableName = tableName;
     this.tableId   = tableId;
     this.tableSituation = tableSituation;
     this.bigBlind = bigBlind;
     this.tablePot = tablePot;
     this.flopCards= flopCards;
     this.currentPos = currentPos;
     this.dealerPos = dealerPos;
     this.seatsCount = seatsCount;
     //
     //seats data
     this.seatsData = [];
     //
     this.tableHandle = null;

     this.ready = false;
   }
   CreateTable()
   {
       //everything of table is on tableBox
       //table box is parent

       if(this.tableHandle == null)
       {
         var tableBox = document.createElement("div");
         tableBox.setAttribute("class","tableBox");
         tableBox.setAttribute("id","table-"+this.tableId);

         var tableHeader = document.createElement("div");
         tableHeader.setAttribute("class","tableHeader");
         tableHeader.innerHTML = this.tableName + " " + this.bigBlind/2 + "/" + this.bigBlind;
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
         flop.setAttribute("id",this.tableId+"-flop");
         table.appendChild(flop);

         var chatBox = document.createElement("div");
         chatBox.setAttribute("id",this.tableId+"-chatbox");
         chatBox.setAttribute("class","chatBox");
         tableContainer.appendChild(chatBox);

         var dealerBtn = document.createElement("div");
         dealerBtn.setAttribute("class","dealerBtn");
         dealerBtn.setAttribute("id",this.tableId+"-dealerBtn");
         table.appendChild(dealerBtn);

         tableContainer.appendChild(table);

         //creating seats
         for(var i = 0 ; i < this.seatsCount ; i++)
         {
             var seat = document.createElement("div");
             seat.setAttribute("class","seat-have-no-player" + " seat"+i);
             seat.setAttribute("id",this.tableId + "-seat-" + i);
             seat.innerHTML = "<a>sit here</a>";

             tableContainer.appendChild(seat);

             this.addReserveHandler(seat,this.tableId,i);
         }
         tableBox.appendChild(tableContainer);
         document.getElementById("container").appendChild(tableBox);

         this.tableHandle = tableBox;
     }
     else
     {
       //showTable();
     }
   }
   show()
   {
     if(this.tableHandle == null)
     {
       this.CreateTable();
     }
     updateViewer();
   }
   updateViewer()
   {
     for(var i = 0; i < this.seatsCount;i++)
      if(this.seatsData[i]!= null){
        SitOnSeat(seatsData[i],i);
        ShowPlayerSituation(seatsData[i],i);
      }
   }
   SitOnSeat(seatData,pos)
   {
       var seat =  document.getElementById(this.tableId+"-seat-"+pos);

       if(seat != null && seatData != null) {
           seat.innerHTML = "";//free seat

           var seatHeader = document.createElement("div");
           seatHeader.innerHTML = "<a>" + seatData.playerName + "</a>";
           seatHeader.setAttribute("class", "seatHeader");
           seat.appendChild(seatHeader);

           var seatImage = document.createElement("div");
           seatImage.setAttribute("class", "seatImage");
           $(seatImage).css("background-image","url("+url+"UsersData/"+seatData.playerName+"/profile.png?time="+new Date().getTime()+")");
           seat.appendChild(seatImage);

           var seatChipCount = document.createElement("div");
           seatChipCount.setAttribute("class","seatChipCount");
           seatChipCount.setAttribute("id",this.tableId+"-seatChipCount-"+pos);

           var seatSituation = document.createElement("div");
           seatSituation.setAttribute("id",this.tableId+"-seatSituation-"+pos);
           seatSituation.setAttribute("class","seatSituation");
           seat.appendChild(seatSituation);

           var playerChip = document.createElement( "div");
           playerChip.setAttribute("class" , "playerChip");
           playerChip.setAttribute("id"    , this.tableId+"-playerChip-"+pos);
           seat.appendChild(playerChip);

           var cardHolder = document.createElement("div");
           cardHolder.setAttribute("id","cardholder-"+this.tableId+"-seat-"+username);
           cardHolder.setAttribute("class","cardHolder");
           seat.appendChild(cardHolder);

           $(seat).removeClass("seat-have-no-player");
           $(seat).addClass("seat-have-player");
           ShowPlayerChip(seatsData.playerChip,pos);
       }
   }
   SitUpSeat(pos)
   {
       var seat = document.getElementById(this.tableId+"-seat-"+pos);
       $(seat).innerHTML = "<a>Seat Here</a>";
       this.addReserveHandler(seat,this.tableId,pos);
       $(seat).removeClass("seat-have-player");
       $(seat).addClass("seat-have-no-player");
   }
   ShowPlayerChip(amount,pos)
   {
     playerChip = this.tableHandle.getElementById(this.tableId+"-playerChip-"+pos);
     if(playerChip != null)
     {
       playerChip.innerHTML = "<a>"+amount+"</a>"
     }
   }
   ShowPlayerSituation(pos)
   {
     var seatSituationElem = this.tableHandle.getElementById(this.tableId+"-seatSituation-"+pos);
     if(seatSituationElem != null)
     {
       seatSituationElem.innerHTML = "<a>" + this.seatsData[pos].playerChip +"</a>";
     }
   }
   showError(message)
   {
     alert(message);
   }
   showControl(elems)
   {
       var table = document.getElementById("table-"+this.tableId);
       var ControlBox = document.createElement("div");
       ControlBox.setAttribute("class","control-box");
       ControlBox.setAttribute("id","ControlBox-"+this.tableId);

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
   addReserveHandler(elem,tableId,pos)
   {
       if(elem != null)
       {
           elem.addEventListener("click",function(){
               conn.send("t=rsv;"+tableId+";"+pos);
           });
       }
   }
   showReserveBox(tableId,pos,minBuyin,maxBuyin)
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
   showFlopCards(cards,tableId)
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
   showTurnCard(card,tableId)
   {
       var flop = document.getElementById(tableId+"-flop");
       name = getCardName(card);
       turn = document.createElement("div");
       turn.setAttribute("class","flopCard");
       turn.setAttribute("id",tableId+"-flopcard" + i);
       $(turn).css("background-image","../lobby/styles/images/cards/"+ name +".png");
       flop.appendChild(turn);
   }
   showRiverCard(card,tableId)
   {
       var flop = document.getElementById(tableId+"-flop");
       name = getCardName(card);
       river = document.createElement("div");
       river.setAttribute("class","flopCard");
       river.setAttribute("id",tableId+"-flopcard" + i);
       $(river).css("background-image","../lobby/styles/images/cards/"+ name +".png");
       flop.appendChild(turn);
   }
   setDealerPos(tableId,Pos)
   {
       var TopPos = $("#"+tableId+"-seat-"+Pos).offset().top;
       var LeftPos = $("#"+tableId+"-seat-"+Pos).offset().left;

       console.log(TopPos + "  :  " +LeftPos);
       $("#"+tableId+"-dealerBtn").offset({top : TopPos , left : LeftPos});
   }
   updateSeatData(playerName,chip,lastMove,timeBank,pos)
   {
     this.seatsData[pos].playerName = playerName;
     this.seatsData[pos].chip = chip;
     this.seatsData[pos].lastMove = lastMove;
     this.seatsData[pos].timeBank = timeBank;
   }
 }
  class TableStorage
  {
    constructor()
    {
      this.AllTables = [];
    }
    addTable(tableName,tableId,tableSituation,bigBlind,tablePot,flopCards,currentPos,dealerPos,seatsCount,seatsData)
    {
      for(var i = 0 ; i < this.AllTables.length; i++)
        if(this.AllTables[i].tableId == tableId)
          return this.AllTables[i];
      return this.AllTables[this.AllTables.length] = new Table(tableName,tableId,tableSituation,bigBlind,tablePot,flopCards,currentPos,dealerPos,seatsCount,seatsData);
    }
    removeTable(tableId)
    {
      for(var i = 0; i < this.AllTables.length ; i++)
      {
        if(this.AllTables[i].tableId == tableId)
        {
          this.AllTables[i] = null;
          break;
        }
      }
    }
    find(tableId)
    {
      for(var i = 0; i < this.AllTables.length ; i++)
      {
        if(this.AllTables[i].tableId == tableId)
          return this.AllTables[i];
      }
      return null;
    }
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
  function deleteControl(tableId)
  {
      var ControlBox = document.getElementById("ControlBox-"+tableId);
      if(ControlBox != null)
      {
          ControlBox.parentNode.removeChild(ControlBox);
      }
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

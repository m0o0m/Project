<div class="news-box" id="top-players">
  <div class="head-news">Top players</div>
  <div class="list-data">
    <div class="list-data-number">1</div>
    <div class="list-data-username">Amirali</div>
    <div class="list-data-amount-have">200$</div>
  </div>
  <div class="list-data">
    <div class="list-data-number">2</div>
    <div class="list-data-username">Ehsan</div>
    <div class="list-data-amount-have">200$</div>
  </div>
  <div class="list-data">
    <div class="list-data-number">3</div>
    <div class="list-data-username">Gholi</div>
    <div class="list-data-amount-have">200$</div>
  </div>
</div>

<div class="news-box" id="tournoments-winners">
  <div class="head-news">Tournoments winners</div>

  <div class="list-data">
    <div class="list-data-win-username">sara</div>
    <div class="list-data-amount">120$</div>
  </div>
  <div class="list-data">
    <div class="list-data-win-username">asghar</div>
    <div class="list-data-amount">20$</div>
  </div>
  <div class="list-data">
    <div class="list-data-win-username">sara</div>
    <div class="list-data-amount">2000$</div>
  </div>

</div>

<div class="news-box" id="bounce-players">
  <div class="head-news">Bounces</div>

  <div class="list-data">
    <div class="list-data-win-username">Xorg</div>
    <div class="list-data-amount">210$</div>
  </div>
  <div class="list-data">
    <div class="list-data-win-username">Zahra</div>
    <div class="list-data-amount">100$</div>
  </div>
  <div class="list-data">
    <div class="list-data-win-username">Shazde</div>
    <div class="list-data-amount">200$</div>
  </div>

</div>
<style>
.news-box
{
  width : 30%;
  height: 90%;
  position: absolute;
  background-color: red;
  top: 50%;
  transform: translateY(-50%);
}
#top-players
{
  left: 1.8%;
}
#bounce-players{
  right: 1.8%;
}
#tournoments-winners
{
  left: 35%;
  background-color: green !important;
}
.head-news
{
  text-align: center;
  font-size: 170%;
  background-color: white;
}
.list-data
{
  width: 100%;
  margin-top: 1%;
  background-color: black;
  color: white;
}
.list-data:hover{
  cursor: default;
  opacity: 0.8;
}
.list-data div
{
  display: inline-block;
}
.list-data-number
{
  width : 10%;
  text-align: center;
}
.list-data-amount-have
{
  width: 39%;
  text-align: center;
}
.list-data-username
{
  width: 40%;
  text-align: center;
}
.list-data-win-username
{
  width: 49%;
  text-align: center;
}
.list-data-amount
{
  width: 49%;
  text-align: center;
}
</style>

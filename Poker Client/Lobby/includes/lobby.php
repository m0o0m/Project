<!DOCTYPE html>
<meta charset="utf-8">
<html>
<head>
    <title><?php echo $GLOBALS["lobby_title"];?></title>
    <link href="<?php echo $GLOBALS["game_url"]."/styles/c.css"?>" rel="stylesheet">
    <link href="<?php echo $GLOBALS["game_url"]."/styles/messagebox.css"?>" rel="stylesheet">
    <link href="<?php echo $GLOBALS["game_url"]."/styles/profile.css"?>" rel="stylesheet">
    <link href="<?php echo $GLOBALS["game_url"]."/styles/tableStyle.css"?>" rel="stylesheet">
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/jquery-1.12.2.min.js"?>"></script>
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/websocket.js"?>"></script>
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/style.js"?>"></script>
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/actions.js"?>"></script>
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/webFunctions.js"?>"></script>
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/initialize.js"?>"></script>
    <script src="<?php echo $GLOBALS["game_url"]."/scripts/TableFunctions.js"?>"></script>
</head>

<body>
    <input id="session_id_store" name="session_id" type="hidden" value="<?php echo $GLOBALS["session_id"];?>">
    <input id="user_id_store" name="user_id" type="hidden" value="<?php echo Session::get("user_id");?>">

    <header>
    </header>

    <div id="main-container">
        <div id="loader"></div>
        <div id="container">
            <div id="lobby-container">
                <div id="sidebar">
                    <div class="sidebar-btn" id="profile-btn"><a>Profile</a></div>
                    <div class="sidebar-btn" id="cashgames-btn"><a>Cash Games</a></div>
                    <div class="sidebar-btn" id="tournoments-btn"><a>Tournoments</a></div>
                    <div class="sidebar-btn" id="sitsandgoes-btn"><a>Sits And Goes</a></div>
                    <div class="sidebar-btn" id="rollet-btn"><a>Rollet</a></div>
                </div>
                <div id="lobby-viewer">

                    <div class="viewer-part" id="profile-viewer">
                        <div id="profile-you">
                            <div id="profile-image">
                                <div id="profile-image-viewer"></div>
                                <div id="edit-profile-image"><a>Change</a></div>
                            </div>
                            <div id="profile-details">
                                </br>
                                <div id="username-details">
                                    User Name : <?php echo Session::get("user_id"); ?>
                                </div>
                                </br>
                                <div id="balance-details">
                                    Balance : ---
                                </div>
                                </br>
                                <div id="ingamechips-detials">
                                    In Game Chips : ---
                                </div>
                            </div>

                            <div id="messages-box">
                                <div id="messages-handle">
                                </div>
                                <div id="messages">
                                    <div id="messages-close-handle"></div>
                                </div>
                            </div>
                        </div>

                        <div id="profile-bottom">
                            <div class="profile-bottom-btn" id="profile-you-btn"><a>You</a></div>
                            <div class="profile-bottom-btn" id="profile-friends-btn"><a>Friends</a></div>
                            <div class="profile-bottom-btn" id="profile-history-btn"><a>History</a></div>
                        </div>

                    </div>

                    <div class="viewer-part" id="cashgames-viewer">
                        <div id="infobox-container-header">
                            <a>Table Name</a>
                            <a>Seats</a>
                            <a>Buyin</a>
                            <a>Stakes</a>
                        </div>
                        <div id="infobox-container"></div>
                    </div>

                    <div class="viewer-part" id="tournoments-viewer">tour</div>
                    <div class="viewer-part" id="sitsandgoes-viewer">sit</div>
                    <div class="viewer-part" id="rollet-viewer">roll</div>

                </div>
            </div>
        </div>
    </div>
    <footer>
    </footer>
</body>
</html>
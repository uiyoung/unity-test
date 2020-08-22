<?php
require './db.php';

// Check connection
if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// variables submited by user
$itemID = $_POST["itemID"];

$path = "./item-icons/" . $itemID . ".png";
$image = file_get_contents($path);

echo $image;

$conn->close();
?>
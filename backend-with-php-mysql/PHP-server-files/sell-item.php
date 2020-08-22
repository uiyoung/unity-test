<?php
require './db.php';

// Check connection
if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// variables submited by user
$id = $_POST["id"];
$userID = $_POST["userID"];
$itemID = $_POST["itemID"];

// first sql
$sql = "SELECT price FROM items WHERE id = ?";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $itemID);

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    // store item price
    $itemPrice = $result->fetch_assoc()["price"];

    $sql2 = "DELETE FROM usersitems WHERE id = ?";
    $stmt2 = $conn->prepare($sql2);
    $stmt2->bind_param("s", $id);
    
    if($stmt2->execute() === TRUE) {
        $sql3 = "UPDATE users SET coins = coins + ? WHERE id = ?";
        $stmt3 = $conn->prepare($sql3);
        $stmt3->bind_param("is", $itemPrice, $userID);

        if ($stmt3->execute() === TRUE) 
            echo "item deleted successfully";
        else 
            echo "Error: " . $sql3 . "<br>" . $conn->error;
    } 
    else
        echo "Error: " . $sql2 . "<br>" . $conn->error;

 } else 
    echo '0';

$stmt3->close();
$stmt2->close();
$stmt->close();
$conn->close();
?>
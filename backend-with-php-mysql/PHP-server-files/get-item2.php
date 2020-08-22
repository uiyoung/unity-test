<?php
require './db.php';

// Check connection
if ($conn->connect_error)
    die('Connection failed: ' . $conn->connect_error);

// variables submited by user
$userID = $_POST["userID"];

// Prepared Statement
$sql = "SELECT * FROM items where id in (SELECT itemID FROM `usersitems` WHERE userID = ?)";
$stmt = $conn->prepare($sql);
$stmt->bind_param("s", $userID);

$stmt->execute();
$result = $stmt->get_result();

if ($result->num_rows > 0) {
    $rows = array();
    while ($row = $result->fetch_assoc()) {
        $rows[] = $row;
    }
    echo json_encode($rows);
} else 
    echo '0';

$stmt->close();
$conn->close();
?>

function ChangeStarColor(value) {
    for (i = 1; i <= 5; i++) {
        if (i <= value) {
            document.getElementById(i).style.color = 'orange';
        }
        else {
            document.getElementById(i).style.color = 'black';
        }
    }
}
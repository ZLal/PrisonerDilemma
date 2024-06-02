
// Cooperate = 0 & Defect = 1

var counter = 0;

function NextIteration() {
    counter = counter + 1;
    if (counter <= 3) {
        return 0;
    }
    else {
        if (counter >= 6) {
            counter = 0;
        }
        return 1;
    }
    return counter;
}

function Reset() {
    counter = 0;
}

function SetResult(player, opponent) {
}

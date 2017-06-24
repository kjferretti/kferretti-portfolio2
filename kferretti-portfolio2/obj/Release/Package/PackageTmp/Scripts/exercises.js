//JavaScript Exercises

//First Exercise
function problemOne() {

    var inputOneA = Number(document.getElementById('inputOneA').value);
    var inputOneB = Number(document.getElementById('inputOneB').value);
    var inputOneC = Number(document.getElementById('inputOneC').value);
    var inputOneD = Number(document.getElementById('inputOneD').value);
    var inputOneE = Number(document.getElementById('inputOneE').value);

    var seriesArray = [inputOneA, inputOneB, inputOneC, inputOneD, inputOneE];

    var currentGreatest = seriesArray[0];
    for (var i = 0; i < seriesArray.length; i++) {
        if (seriesArray[i] > currentGreatest) {
            currentGreatest = seriesArray[i];
        }
    }
    var greatest = currentGreatest;

    var currentLeast = seriesArray[0];
    for (var i = 0; i < seriesArray.length; i++) {
        if (seriesArray[i] < currentLeast) {
            currentLeast = seriesArray[i];
        }
    }
    var least = currentLeast;

    var sum = 0;
    var product = 1;
    for (var i = 0; i < seriesArray.length; i++) {
        sum += seriesArray[i];
        product *= seriesArray[i];
    }

    var mean = sum / seriesArray.length;

    document.getElementById('resultOne').innerHTML = 'Least Number: ' + least + ' Greatest Number: ' + greatest + ' Mean: ' + mean + ' Sum: ' + sum + ' Product: ' + product;

}
//Second Exercise
function problemTwo() {
    var inputTwo = Number(document.getElementById('inputTwo').value);

    if (inputTwo < 0 || inputTwo > 170) {
        document.getElementById('resultTwo').innerHTML = 'Invalid input';
    }

    else if (inputTwo > 0) {

        var factorial = 1;

        while (inputTwo != 0) {
            factorial *= inputTwo;
            inputTwo = inputTwo - 1;
        }

        document.getElementById('resultTwo').innerHTML = 'Factorial: ' + factorial;
    }

    else {
        document.getElementById('resultTwo').innerHTML = 'Factorial: ' + 1;
    }
}

//Third Exercise
function problemThree() {

    var inputThreeA = Number(document.getElementById('inputThreeA').value);
    var inputThreeB = Number(document.getElementById('inputThreeB').value);

    var fizzBuzzArray = [];

    if (1 <= inputThreeA <= 100 && 1 <= inputThreeB <= 100) {

        for (var i = 1, l = 0; i <= 100; i++, l++) {

            if ((i % inputThreeA) == 0 && (i % inputThreeB) == 0) {
                fizzBuzzArray[l] = ' ' + 'FizzBuzz' + ' ';
            }

            else if ((i % inputThreeA) == 0) {
                fizzBuzzArray[l] = ' ' + 'Fizz' + ' ';
            }

            else if ((i % inputThreeB) == 0) {
                fizzBuzzArray[l] = ' ' + 'Buzz' + ' ';
            }

            else {
                fizzBuzzArray[l] = ' ' + i + ' ';
            }
        }

        document.getElementById('resultThree').innerHTML = fizzBuzzArray;
    }

    else {
        document.getElementById('resultThree').innerHTML = 'Invalid input';
    }
}

//Fourth Exercise
function problemFour() {

    var inputFour = document.getElementById('inputFour').value;
    var inputFourRev = '';
    //var newInputFour = '';

    if (inputFour == '') {
        document.getElementById('resultFour').innerHTML = 'Please write something before submitting.'

    } else {

        var lowercaseLetters = 'abcdefghijklmnopqrstuvwxyz';
        var capitalLetters = 'ABCDEFGHIJKLMNOPQRSTUVWQYZ';

        for (var i = 0; i < capitalLetters.length; i++) {
            for (var j = 0; j < inputFour.length; j++) {
                if (inputFour[j] === capitalLetters[i]) {
                    inputFour = inputFour.substr(0, j) + lowercaseLetters[i] + inputFour.substr(j + 1);
                }
            }
        }

        for (var i = inputFour.length - 1; i >= 0; i--) {
            inputFourRev += inputFour[i];
        }



        //var innerFour = inputFour.substr(1, inputFourRev.length - 2);
        //var innerRev = inputFourRev.substr(1, inputFourRev.length - 2);

        if (inputFour === inputFourRev) {
            document.getElementById('resultFour').innerHTML = 'This word is a palindrome, congrats!';
        }

            //if (inputFour[0] === inputFourRev[inputFourRev.length - 1] && innerFour === innerRev) {
            //    document.getElementById('resultFour').innerHTML = 'This word is a palindrome, congrats!';
            //}

        else {
            document.getElementById('resultFour').innerHTML = 'This word is not a palindrome, sorry!';
        }
    }
}
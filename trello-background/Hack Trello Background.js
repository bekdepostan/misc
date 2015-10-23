// ==UserScript==
// @name         Hack Trello Background 
// @namespace    http://hack.trello.background/
// @version      0.1
// @description  customize the backgroud of Trello
// @author       None
// @include      https://trello.com/b/*
// @grant        None
// ==/UserScript==
var boards = [["test","background-image","url('http://pandaweb.gr/wp-content/uploads/2014/09/office-desk-background-2.jpg')"],
              ["hobby","background-image","PINK"]];
var currBoard = "U-N-K-N-O-W-N";

function changeBg(){
    if ($('title').text().toLowerCase().indexOf(currBoard) >= 0){
        return;
    }
    var body = $("BODY");
    if(body !== null){
        for(var i in boards){
            var b = boards[i];
            if(window.location.href.toLowerCase().indexOf(b[0]) != -1){
                console.log("change bg");
                currBoard = b[0];
                body.css(b[1],b[2]);
                break;
            }
        }
    }
}

window.addEventListener('load', function() {
    changeBg();
    $('title').bind("DOMSubtreeModified",changeBg)
}, false);
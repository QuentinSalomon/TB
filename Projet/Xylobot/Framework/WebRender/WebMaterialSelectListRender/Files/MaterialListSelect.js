var listSelectable<%id%> = document.getElementById("listSelectable<%id%>");
var inputSelected<%id%> = document.getElementById("inputSelected<%id%>");
var listName<%id%> = [<%values%>];

function ClickLabel(){
    inputSelected<%id%>.value = innerHTML;
}

function AddItemSelectable(i){
    <!--Ajoute la valeur à la liste-->
    var new_name = document.createElement("label");
    <!--new_name.appendChild(document.createTextNode(listName<%id%>[i]));-->
    new_name.innerHTML = listName<%id%>[i];
    new_name.class = "itemListLabelClickable";
    new_name.onclick = function() {
        inputSelected<%id%>.value = this.innerHTML;
        server.write("<%path%>", inputSelected<%id%>.value, function(value){});
    }
    <!--new_name.onClick = ClickLabel;(new_name.innerHTML);-->
    listSelectable<%id%>.appendChild(new_name);
    
    listSelectable<%id%>.appendChild(document.createElement("hr"));
};

function ActualiseSelectList(){
    <!--$( "listSelectable<%id%>" ).empty();-->
    listSelectable<%id%>.innerHTML = "";
    <!--Création de la liste-->
    for (var i = 0; i < listName<%id%>.length; i++){
        AddItemSelectable(i);
    }
}

ActualiseSelectList();


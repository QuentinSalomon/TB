var parent = document.getElementById("p");
var myArray = [<%values%>];

function AddItem(i){
    <!--Ajoute la valeur à la liste-->
    var new_name = document.createElement("label");
    new_name.appendChild(document.createTextNode(myArray[i]));
    parent.appendChild(new_name);
    
    parent.appendChild(document.createElement("hr"));
};

function Actualise(){
    $( "p" ).empty();
    <!--Création de la liste-->
    for (var i = 0; i < myArray.length; i++){
        AddItem(i);
    }
}

Actualise();


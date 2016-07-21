var parent<%id%> = document.getElementById("list<%id%>");
var listName<%id%> = [<%values%>];

function AddItemShow(i){
    <!--Ajoute la valeur à la liste-->
    var new_name = document.createElement("label");
    new_name.appendChild(document.createTextNode(listName<%id%>[i]));
    parent<%id%>.appendChild(new_name);
    
    parent<%id%>.appendChild(document.createElement("hr"));
};

function ActualiseShowList(){
    <!--$( "list<%id%>" ).empty();-->
    parent<%id%>.innerHTML = "";
    <!--Création de la liste-->
    for (var i = 0; i < listName<%id%>.length; i++){
        AddItemShow(i);
    }
}

ActualiseShowList();


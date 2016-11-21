/*jslint browser: true, devel: true, white: true, eqeq: true, plusplus: true, sloppy: true, vars: true*/
/*global $ */

/*************** General ***************/

var updateOutput = function (e) {
    var list = e.length ? e : $(e.target),
        output = list.data('output');
  if (window.JSON) {
    if (output) {
        output.val(window.JSON.stringify(list.nestable('serialize')));
        return window.JSON.stringify(list.nestable('serialize'));
    }
  } else {
    alert('JSON browser support required for this page.');
  }
};

var nestableList = $("#nestable > .dd-list");

/***************************************/


/*************** Delete ***************/

var deleteFromMenuHelper = function (target) {
    target.fadeOut(function () {
        target.remove();
        updateOutput($('#nestable').data('output', $('#json-output')));
    });
};

var deleteFromMenu = function () {
  var targetId = $(this).data('owner-id');
  var target = $('[data-id="' + targetId + '"]');

  var result = confirm("Delete " + target.data('name') + " and all its subitems ?");
  if (!result) {
    return;
  }

  // Remove children (if any)
  target.find("li").each(function () {
    deleteFromMenuHelper($(this));
  });

  // Remove parent
  deleteFromMenuHelper(target);

  // update JSON
  updateOutput($('#nestable').data('output', $('#json-output')));
};

/***************************************/


/*************** Edit ***************/

var menuEditor = $("#menu-editor");
var editButton = $("#editButton");
var editInputName = $("#editInputName");
var editInputUrl = $("#editInputUrl");
var currentEditName = $("#currentEditName");

// Prepares and shows the Edit Form
var prepareEdit = function () {
  var targetId = $(this).data('owner-id');
  var target = $('[data-id="' + targetId + '"]');

  editInputName.val(target.data("name"));
  editInputUrl.val(target.data("url"));
  currentEditName.html(target.data("name"));
  editButton.data("owner-id", target.data("id"));

  console.log("[INFO] Editing Menu Item " + editButton.data("owner-id"));

  menuEditor.fadeIn();
};

// Edits the Menu item and hides the Edit Form
var editMenuItem = function () {
  var targetId = $(this).data('owner-id');
  var target = $('[data-id="' + targetId + '"]');

  var newName = editInputName.val();
  var newUrl = editInputUrl.val();

  target.data("name", newName);
  target.data("url", newUrl);

  if (target.data('new') == 0)
      target.data("edit", 1);

  target.find("> .dd-handle").html(newName);

  menuEditor.fadeOut();

  // update JSON
  updateOutput($('#nestable').data('output', $('#json-output')));
};

/***************************************/


/*************** Add ***************/

var newIdCount = 1;

var addToMenu = function () {
  var newName = $("#addInputName").val();
  var newUrl = $("#addInputUrl").val();
  var newId = newIdCount;

  nestableList.append(
    '<li class="dd-item" ' +
    'data-id="' + newId + '" ' +
    'data-name="' + newName + '" ' +
    'data-url="' + newUrl + '" ' +
    'data-new="1" ' +
    'data-edit="0">' +
    '<div class="dd-handle">' + newName + '</div> ' +
    '<span class="button-delete btn btn-default btn-xs pull-right" ' +
    'data-owner-id="' + newId + '"> ' +
    '<i class="glyphicon glyphicon-remove" aria-hidden="true"></i> ' +
    '</span>' +
    '<span class="button-edit btn btn-default btn-xs pull-right" ' +
    'data-owner-id="' + newId + '">' +
    '<i class="glyphicon glyphicon-edit" aria-hidden="true"></i>' +
    '</span>' + //'<ol class="dd-list">' + '<ol>'+
    '</li>'
  );

  newIdCount++;

  // update JSON
  updateOutput($('#nestable').data('output', $('#json-output')));

  // set events
  $("#nestable .button-delete").on("click", deleteFromMenu);
  $("#nestable .button-edit").on("click", prepareEdit);
};



/***************************************/

/*************** Load json to menu *****/

function buildItem(item) {
    var html = '<li class="dd-item" data-id="' + item.id + '" data-name="' + item.name + '" data-url="' + item.url + '" data-new="0" data-edit="0">';
    if (item.children) {
        html += '<button data-action="collapse" type="button">"collapse"</button>' + '<button data-action="expand" type="button" style="display: none;">Expand</button>'
    }
    html += '<div class="dd-handle">' + item.name + '</div> ' +
            '<span class="button-delete btn btn-default btn-xs pull-right" data-owner-id="' + item.id + '"> ' +
                '<i class="glyphicon glyphicon-remove" aria-hidden="true"></i> ' +
            '</span>' +
            '<span class="button-edit btn btn-default btn-xs pull-right" data-owner-id="' + item.id + '">' +
                '<i class="glyphicon glyphicon-edit" aria-hidden="true"></i>' +
            '</span>';
    if (item.children) {
        html += "<ol class='dd-list'>";
        $.each(item.children, function (index, sub) { html += buildItem(sub); });
        html += "</ol>";
    }
    html += "</li>";
    return html;
}
function LoadJSONtoMenu(jsn) {
    $('#nestableContainer').empty();
    
    $.each(JSON.parse(jsn), function (index, item) {
        $('#nestableContainer').append(buildItem(item));
    });
}

/***************************************/

$(document).ready(function () {

  // output initial serialised data
  updateOutput($('#nestable').data('output', $('#json-output')));

  // set onclick events
  editButton.on("click", editMenuItem);

  $("#nestable .button-delete").on("click", deleteFromMenu);

  $("#nestable .button-edit").on("click", prepareEdit);

  $("#menu-editor").submit(function (e) {
    e.preventDefault();
  });

  $("#menu-add").submit(function (e) {
    e.preventDefault();
    addToMenu();
  });

  $("#saveMenuButton").click(function () {
      var jsn = updateOutput($('#nestable').data('output', $('#json-output')));
      var id = $('#menuDropList').find('option:selected').data('id');
      $.ajax({
          url: "/Admin/SaveMenu",
          method: 'POST',
          data: { id:id , json: jsn },
          success: function () { Toast('saved'); },
          error: ShowError,
      });
  });
});

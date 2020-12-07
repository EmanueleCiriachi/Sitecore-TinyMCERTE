(function () {
	var sitecoretinymcebuttons = (function () {
		'use strict';

		function InsertSitecoreLink(editor) {
			var id;

			// We grab the selected portion from the TinyMCE editor
			var selection = editor.selection.getContent();

			// If we do have a selection, we try to get the Sitecore ID associated with it
			if (selection) {
    			id = GetMediaID(selection);
			}

			// link to media in form of <a href="-/media/CC2393E7CA004EADB4A155BE4761086B.ashx">...</a>
			if (!id) {
			    var regex = /~\/media\/([\w\d]+)\.ashx/;
			    var match = regex.exec(selection);
			    if (match && match.length >= 1 && match[1]) {
			      	id = match[1];
			    }
			}

			// If all else fails, we get the current item's ID
			if (!id) {
				id = scItemID;
			}

			// We get the Sitecore Link Picker, we create a modal panel and we load it in it
			var url = "/sitecore/shell/default.aspx?xmlcontrol=TinyMCE.InsertLink&la=" + scLanguage + "&fo=" + id + (scDatabase ? "&databasename=" + scDatabase : "");
			
			var data = [];
			data.push('<iframe name="Window" src="');
			data.push(url);
			data.push('" frameborder="0" style="width: 100%; height: 100%; border: 0px; top: -10000px;"></iframe>');

			var container = window.$sc('<div />').appendTo('body').modal({
				modalClass: "editorModal"
			});

			window.$sc(".jquery-modal.blocker.current").append(window.$sc('<div class="sitecoreInsertLink" />').html(data.join('')));
		}

		function InsertSitecoreImage(editor) {
			var id;

			// We grab the selected portion from the TinyMCE editor
			var selection = editor.selection.getContent();

			// If we do have a selection, we try to get the Sitecore ID associated with it
			if (selection) {
    			id = GetMediaID(selection);
			}

			// link to media in form of <a href="-/media/CC2393E7CA004EADB4A155BE4761086B.ashx">...</a>
			if (!id) {
			    var regex = /~\/media\/([\w\d]+)\.ashx/;
			    var match = regex.exec(selection);
			    if (match && match.length >= 1 && match[1]) {
			      	id = match[1];
			    }
			}

			// If all else fails, we get the current item's ID
			if (!id) {
				id = scItemID;
			}

			// We get the Sitecore Link Picker, we create a modal panel and we load it in it
			var url = "/sitecore/shell/default.aspx?xmlcontrol=TinyMCE.InsertImage&la=" + scLanguage + "&fo=" + id + (scDatabase ? "&databasename=" + scDatabase : "");
			
			var data = [];
			data.push('<iframe name="Window" src="');
			data.push(url);
			data.push('" frameborder="0" style="width: 100%; height: 100%; border: 0px; top: -10000px;"></iframe>');

			var container = window.$sc('<div />').appendTo('body').modal({
				modalClass: "editorModal"
			});

			window.$sc(".jquery-modal.blocker.current").append(window.$sc('<div class="sitecoreInsertImage" />').html(data.join('')));
		}

		function displaySitecoreInsertLink(data) {
			var container = window.$sc('<div />').html(data).appendTo('body').modal({
				modalClass: "sitecoreInsertLink"
			});
		}

		function GetMediaID(html) {
			var id = null;
			var list = prefixes.split('|');

			if(!list) {
				id = GetIDByMediaPrefix('~\\/media\\/([\\w\\d]+)\\.ashx', html);
			} else {
				for(var i = 0; i < list.length; i++) {
					if(list[i] != '') {
						id = GetIDByMediaPrefix(list[i] +'([\\w\\d]+)\\.ashx', html);
						if(id) {
							break;
						}
					}
				}
			}

			return id;
		}

		var global = tinymce.util.Tools.resolve('tinymce.PluginManager');

		var register = function (editor) {
			var mceeditor = editor;
            editor.ui.registry.addButton('insertsitecorelink', {
				type: 'button',
				title: 'Insert Sitecore Link',
				icon: "link",
				onAction: function () {
					InsertSitecoreLink(mceeditor);
				}
			});
            editor.ui.registry.addButton('insertsitecoreimage', {
				type: 'button',
				title: 'Insert Sitecore Image',
				icon: "image",
				onAction: function () {
					InsertSitecoreImage(mceeditor);
				}
			});
		}

		var $_c8389vmytubtytpn = { register: register };

		global.add('sitecoretinymcebuttons', function (editor) {
			$_c8389vmytubtytpn.register(editor);
		});

		function Plugin () {
		}
		return Plugin;

	})();
})();
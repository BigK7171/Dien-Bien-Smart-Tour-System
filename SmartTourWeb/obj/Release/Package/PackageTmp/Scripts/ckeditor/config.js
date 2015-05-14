/**
 * @license Copyright (c) 2003-2014, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
    config.language = 'vi';
    config.extraPlugins = 'imagebrowser';
    // config.uiColor = '#AADC6E';
    var duong_dan = '/scripts/';
    config.filebrowserBrowseUrl = duong_dan + 'ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = duong_dan + 'ckfinder/ckfinder.html?type=Files';
    config.filebrowserFlashBrowseUrl = duong_dan + 'ckfinder/ckfinder.html?type=Files';
    config.filebrowserUploadUrl = duong_dan + 'ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = duong_dan + 'ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files';
    config.filebrowserFlashUploadUrl = duong_dan + '/ckfinder/core/connector/php/connector.php?command=QuickUpload&type=Files';

};

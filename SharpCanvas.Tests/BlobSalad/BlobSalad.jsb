<?xml version="1.0" encoding="utf-8"?>
<project path="" name="BlobSalad" author="Jack Slocum" version="1.0" copyright="$projectName&#xD;&#xA;Copyright(c) 2006, $author.&#xD;&#xA;&#xD;&#xA;This code is licensed under BSD license. Use it as you wish, &#xD;&#xA;but keep this copyright intact." output="$project" source="False" source-dir="$output\source" minify="False" min-dir="$output\build" doc="False" doc-dir="$output\docs" master="true" master-file="$output\yui-ext.js" zip="true" zip-file="$output\yuo-ext.$version.zip">
  <directory name="" />
  <file name="blobsalad.js" path="" />
  <file name="JSCOnly.js" path="" />
  <file name="JSCOnlyEnd.js" path="" />
  <target name="combined" file="$output\combined.js" debug="True" shorthand="False" shorthand-list="YAHOO.util.Dom.setStyle&#xD;&#xA;YAHOO.util.Dom.getStyle&#xD;&#xA;YAHOO.util.Dom.getRegion&#xD;&#xA;YAHOO.util.Dom.getViewportHeight&#xD;&#xA;YAHOO.util.Dom.getViewportWidth&#xD;&#xA;YAHOO.util.Dom.get&#xD;&#xA;YAHOO.util.Dom.getXY&#xD;&#xA;YAHOO.util.Dom.setXY&#xD;&#xA;YAHOO.util.CustomEvent&#xD;&#xA;YAHOO.util.Event.addListener&#xD;&#xA;YAHOO.util.Event.getEvent&#xD;&#xA;YAHOO.util.Event.getTarget&#xD;&#xA;YAHOO.util.Event.preventDefault&#xD;&#xA;YAHOO.util.Event.stopEvent&#xD;&#xA;YAHOO.util.Event.stopPropagation&#xD;&#xA;YAHOO.util.Event.stopEvent&#xD;&#xA;YAHOO.util.Anim&#xD;&#xA;YAHOO.util.Motion&#xD;&#xA;YAHOO.util.Connect.asyncRequest&#xD;&#xA;YAHOO.util.Connect.setForm&#xD;&#xA;YAHOO.util.Dom&#xD;&#xA;YAHOO.util.Event">
    <include name="JSCOnly.js" />
    <include name="blobsalad.js" />
    <include name="JSCOnlyEnd.js" />
  </target>
</project>
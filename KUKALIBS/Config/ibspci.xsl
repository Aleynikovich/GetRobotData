<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0"
   xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
   xmlns:msxsl="urn:schemas-microsoft-com:xslt"
   xmlns:user="http://www.kuka.com/scripts">

   <msxsl:script implements-prefix="user" language="C#">
      public string ExtractDirectory(string path)
      {
         return path.Substring(0, path.LastIndexOf('\\') + 1);
      }
   </msxsl:script>


   <xsl:output method="xml" version="1.0" encoding="UTF-8" indent="yes" media-type="text/xml"/>
   <xsl:variable name="xmlFile">ibspci.xml</xsl:variable>
   <xsl:template match="Driver">
      <xsl:variable name="xpath" select="name()"/>
      <RootNode
            name="IODriverManagement#IbsConfigTitle"
            readPermission="all"
            editPermission="all"
            Appearance="visible">
         <Source
               xmlFile="{$xmlFile}"
               xPath="{$xpath}">
         </Source>
         <xsl:apply-templates select="DriverInstance">
            <xsl:with-param name="parentXPath" select="$xpath" />
         </xsl:apply-templates>
      </RootNode>
   </xsl:template>
   <xsl:template match="DriverInstance">
      <xsl:param name="parentXPath"/>
      <xsl:variable name="xpath" select="concat($parentXPath,'/',name(),'[',position(),']')"/>
      <!--<ValueNode
             name="busInstanceName"
             readPermission="all"
             editPermission="all"
             type="string"
             value="{@busInstanceName}">
         <Source
					xmlFile="{$xmlFile}"
					xPath="{$xpath}/@busInstanceName">
         </Source>
      </ValueNode>-->
      <xsl:apply-templates select="CMD_CONFIGURATION">
         <xsl:with-param name="parentXPath" select="$xpath" />
      </xsl:apply-templates>
   </xsl:template>
   <xsl:template match="INTERBUS">
      <xsl:param name="parentXPath"/>
      <xsl:variable name="xpath" select="concat($parentXPath,'/',name(),'[',position(),']')"/>
      <xsl:apply-templates select="CMD_CONFIGURATION">
         <xsl:with-param name="parentXPath" select="$xpath" />
      </xsl:apply-templates>
   </xsl:template>

   <xsl:template match="CMD_CONFIGURATION">
      <xsl:param name="parentXPath"/>
      <xsl:variable name="xpath" select="concat($parentXPath,'/',name(),'[',position(),']')"/>
      <xsl:variable name="cmdDir" select="user:ExtractDirectory(@CMD_FILE)" />
      <GroupNode
            name="{@Name}"
            readPermission="all"
            editPermission="all"
            Appearance="content">
         <Source
               xmlFile="{$xmlFile}"
               xPath="{$xpath}">
         </Source>
         <ValueNode
            name="InterbusConfig#FieldCmdFolder"
            readPermission="all"
	         editPermission="all"
            type="static"
            value="{$cmdDir}">
            <Source
                  xmlFile="{$xmlFile}"
                  xPath="{$xpath}/@CMD_FILE">
            </Source>
         </ValueNode>
         <ValueNode
           name="InterbusConfig#FieldCmdFilename"
           readPermission="all"
           editPermission="all"
           type="string"
           value="{substring-after(@CMD_FILE, user:ExtractDirectory(@CMD_FILE))}">
            <Source xmlFile="{$xmlFile}"
                    xPath="{$xpath}/@CMD_FILE"
		              writeExpression="concat('{$cmdDir}', $value)">
            </Source>
            <Mask maskValue="^[a-zA-Z0-9 _,;!@'=%#~&amp;\-\+\(\)\$\^\.]+$" />
         </ValueNode>
      </GroupNode>
   </xsl:template>
</xsl:stylesheet>

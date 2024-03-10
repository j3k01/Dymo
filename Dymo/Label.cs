using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DymoTest
{
    public class Label
    {
        public string Xml { get; private set; }

        public Label(string text)
        {
            Xml = GenerateXml(text);
        }

        private string GenerateXml(string text)
        {
            return $@"<?xml version=""1.0"" encoding=""utf-8""?>
<DieCutLabel Version=""8.0"" Units=""twips"">
  <PaperOrientation>Landscape</PaperOrientation>
  <Id>LargeShipping</Id>
  <PaperName>30256 Shipping</PaperName>
  <DrawCommands>
    <RoundRectangle X=""0"" Y=""0"" Width=""1331"" Height=""2715"" Rx=""270"" Ry=""270""/>
  </DrawCommands>
  <ObjectInfo>
    <TextObject>
      <Name>TEXT</Name>
      <ForeColor Alpha=""255"" Red=""0"" Green=""0"" Blue=""0""/>
      <BackColor Alpha=""0"" Red=""255"" Green=""255"" Blue=""255""/>
      <LinkedObjectName></LinkedObjectName>
      <Rotation>Rotation0</Rotation>
      <IsMirrored>False</IsMirrored>
      <IsVariable>False</IsVariable>
      <HorizontalAlignment>Left</HorizontalAlignment>
      <VerticalAlignment>Middle</VerticalAlignment>
      <TextFitMode>AlwaysFit</TextFitMode>
      <UseFullFontHeight>True</UseFullFontHeight>
      <Verticalized>False</Verticalized>
      <StyledText>
        <Element>
          <String></String>
          <Attributes>
            <Font Family=""Helvetica"" Size=""13"" 
            	Bold=""False"" Italic=""False"" Underline=""False"" Strikeout=""False""/>
            <ForeColor Alpha=""255"" Red=""0"" Green=""0"" Blue=""0""/>
          </Attributes>
        </Element>
        <Element>
          <String>{text}</String>
          <Attributes>
            <Font Family=""Helvetica"" Size=""13"" 
            	Bold=""False"" Italic=""False"" Underline=""False"" Strikeout=""False""/>
            <ForeColor Alpha=""255"" Red=""0"" Green=""0"" Blue=""0""/>
          </Attributes>
        </Element>
      </StyledText>
    </TextObject>
    <Bounds X=""335.9998"" Y=""57.6001"" Width=""1337.6"" Height=""1192""/>
  </ObjectInfo>
</DieCutLabel>";
        }
    }
}

Imports System
Imports System.Collections.Generic
Imports System.Xml
Imports System.Xml.Schema
Imports System.Xml.Serialization

Namespace DialoguerCore
	' Token: 0x02000B80 RID: 2944
	<XmlRoot("dictionary")>
	Public Class DialoguerSerializableDictionary(Of TKey, TValue)
		Inherits Dictionary(Of TKey, TValue)
		Implements IXmlSerializable

		' Token: 0x060046CF RID: 18127 RVA: 0x0024FAE9 File Offset: 0x0024DEE9
		Public Function GetSchema() As XmlSchema Implements System.Xml.Serialization.IXmlSerializable.GetSchema
			Return Nothing
		End Function

		' Token: 0x060046D0 RID: 18128 RVA: 0x0024FAEC File Offset: 0x0024DEEC
		Public Sub ReadXml(reader As XmlReader) Implements System.Xml.Serialization.IXmlSerializable.ReadXml
			Dim xmlSerializer As XmlSerializer = New XmlSerializer(GetType(TKey))
			Dim xmlSerializer2 As XmlSerializer = New XmlSerializer(GetType(TValue))
			Dim isEmptyElement As Boolean = reader.IsEmptyElement
			reader.Read()
			If isEmptyElement Then
				Return
			End If
			While reader.NodeType <> XmlNodeType.EndElement
				reader.ReadStartElement("item")
				reader.ReadStartElement("key")
				Dim tkey As TKey = CType(CObj(xmlSerializer.Deserialize(reader)), TKey)
				reader.ReadEndElement()
				reader.ReadStartElement("value")
				Dim tvalue As TValue = CType(CObj(xmlSerializer2.Deserialize(reader)), TValue)
				reader.ReadEndElement()
				MyBase.Add(tkey, tvalue)
				reader.ReadEndElement()
				reader.MoveToContent()
			End While
			reader.ReadEndElement()
		End Sub

		' Token: 0x060046D1 RID: 18129 RVA: 0x0024FBA4 File Offset: 0x0024DFA4
		Public Sub WriteXml(writer As XmlWriter) Implements System.Xml.Serialization.IXmlSerializable.WriteXml
			Dim xmlSerializer As XmlSerializer = New XmlSerializer(GetType(TKey))
			Dim xmlSerializer2 As XmlSerializer = New XmlSerializer(GetType(TValue))
			For Each tkey As TKey In MyBase.Keys
				writer.WriteStartElement("item")
				writer.WriteStartElement("key")
				xmlSerializer.Serialize(writer, tkey)
				writer.WriteEndElement()
				writer.WriteStartElement("value")
				Dim tvalue As TValue = MyBase(tkey)
				xmlSerializer2.Serialize(writer, tvalue)
				writer.WriteEndElement()
				writer.WriteEndElement()
			Next
		End Sub
	End Class
End Namespace

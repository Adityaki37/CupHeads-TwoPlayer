Imports System
Imports System.IO
Imports System.Text
Imports System.Xml.Serialization

' Token: 0x02000384 RID: 900
Public Class Xml
	' Token: 0x06000AA6 RID: 2726 RVA: 0x0007F9A4 File Offset: 0x0007DDA4
	Public Shared Function Serialize(obj As Object) As String
		Dim stringBuilder As StringBuilder = New StringBuilder()
		Dim xmlSerializer As XmlSerializer = New XmlSerializer(obj.[GetType]())
		Using textWriter As TextWriter = New StringWriter(stringBuilder)
			xmlSerializer.Serialize(textWriter, obj)
		End Using
		Return stringBuilder.ToString()
	End Function

	' Token: 0x06000AA7 RID: 2727 RVA: 0x0007F9FC File Offset: 0x0007DDFC
	Public Shared Function Deserialize(Of T)(xml As String) As T
		Dim t As T = Nothing
		Dim xmlSerializer As XmlSerializer = New XmlSerializer(GetType(T))
		Using textReader As TextReader = New StringReader(xml)
			t = CType(CObj(xmlSerializer.Deserialize(textReader)), T)
		End Using
		Return t
	End Function
End Class

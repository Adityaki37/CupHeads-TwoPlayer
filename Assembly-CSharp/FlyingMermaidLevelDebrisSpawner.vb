Imports System
Imports UnityEngine

' Token: 0x02000682 RID: 1666
Public Class FlyingMermaidLevelDebrisSpawner
	Inherits ScrollingSpriteSpawner

	' Token: 0x06002327 RID: 8999 RVA: 0x0014A344 File Offset: 0x00148744
	Protected Overrides Sub OnSpawn(obj As GameObject)
		MyBase.OnSpawn(obj)
		Dim component As FlyingMermaidLevelFloater = obj.GetComponent(Of FlyingMermaidLevelFloater)()
		If component IsNot Nothing Then
			component.trackingWater = Me.trackingWater
		End If
	End Sub

	' Token: 0x04002BCC RID: 11212
	<SerializeField()>
	Private trackingWater As GameObject
End Class

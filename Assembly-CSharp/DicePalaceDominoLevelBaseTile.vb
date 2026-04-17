Imports System
Imports UnityEngine

' Token: 0x020005B1 RID: 1457
Public Class DicePalaceDominoLevelBaseTile
	Inherits AbstractCollidableObject

	' Token: 0x1700035E RID: 862
	' (get) Token: 0x06001C34 RID: 7220 RVA: 0x00102DC8 File Offset: 0x001011C8
	' (set) Token: 0x06001C35 RID: 7221 RVA: 0x00102DD0 File Offset: 0x001011D0
	Public Property currentColourIndex As Integer

	' Token: 0x1700035F RID: 863
	' (get) Token: 0x06001C36 RID: 7222 RVA: 0x00102DD9 File Offset: 0x001011D9
	' (set) Token: 0x06001C37 RID: 7223 RVA: 0x00102DE1 File Offset: 0x001011E1
	Public Property isActivated As Boolean

	' Token: 0x06001C38 RID: 7224 RVA: 0x00102DEA File Offset: 0x001011EA
	Public Overridable Sub InitTile()
		Me.isActivated = True
	End Sub

	' Token: 0x06001C39 RID: 7225 RVA: 0x00102DF3 File Offset: 0x001011F3
	Public Overridable Sub InitTile(parent As DicePalaceDominoLevelFloor, properties As LevelProperties.DicePalaceDomino)
		Me.properties = properties
		Me.isActivated = True
	End Sub

	' Token: 0x06001C3A RID: 7226 RVA: 0x00102E03 File Offset: 0x00101203
	Public Overridable Sub DeactivateTile()
		Me.isActivated = False
	End Sub

	' Token: 0x06001C3B RID: 7227 RVA: 0x00102E0C File Offset: 0x0010120C
	Protected Overrides Sub OnDestroy()
		MyBase.OnDestroy()
	End Sub

	' Token: 0x04002541 RID: 9537
	<SerializeField()>
	Protected colours As Sprite()

	' Token: 0x04002542 RID: 9538
	Protected properties As LevelProperties.DicePalaceDomino
End Class

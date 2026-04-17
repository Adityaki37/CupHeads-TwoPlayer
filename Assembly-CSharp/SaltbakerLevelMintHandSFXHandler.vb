Imports System
Imports UnityEngine

' Token: 0x020007CF RID: 1999
Public Class SaltbakerLevelMintHandSFXHandler
	Inherits MonoBehaviour

	' Token: 0x06002D64 RID: 11620 RVA: 0x001AC9A4 File Offset: 0x001AADA4
	Private Sub AniEvent_SFXLeafRustle()
		Me.main.SFXLeafRustle()
	End Sub

	' Token: 0x06002D65 RID: 11621 RVA: 0x001AC9B1 File Offset: 0x001AADB1
	Private Sub AniEvent_SFXLaunchThrow()
		Me.main.SFXLaunchThrow()
	End Sub

	' Token: 0x040035EC RID: 13804
	<SerializeField()>
	Private main As SaltbakerLevelSaltbaker
End Class

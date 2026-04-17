Imports System
Imports System.Collections
Imports UnityEngine

' Token: 0x020005E7 RID: 1511
Public Class DicePalaceTestLevelTest
	Inherits LevelProperties.DicePalaceTest.Entity

	' Token: 0x06001E01 RID: 7681 RVA: 0x00113EAC File Offset: 0x001122AC
	Public Iterator Function start_it_cr() As IEnumerator
		MyBase.StartCoroutine(Me.shoot_right())
		MyBase.StartCoroutine(Me.shoot_right_2())
		MyBase.StartCoroutine(Me.shoot_left())
		Yield Nothing
		Return
	End Function

	' Token: 0x06001E02 RID: 7682 RVA: 0x00113EC8 File Offset: 0x001122C8
	Private Iterator Function shoot_right() As IEnumerator
		While True
			Me.basic.Create(Me.shoot1.transform.position, 0F, Me.speed)
			Yield CupheadTime.WaitForSeconds(Me, 2F)
		End While
		Return
	End Function

	' Token: 0x06001E03 RID: 7683 RVA: 0x00113EE4 File Offset: 0x001122E4
	Private Iterator Function shoot_right_2() As IEnumerator
		While True
			Me.basic.Create(Me.shoot3.transform.position, 0F, Me.speed)
			Yield CupheadTime.WaitForSeconds(Me, 2F)
		End While
		Return
	End Function

	' Token: 0x06001E04 RID: 7684 RVA: 0x00113F00 File Offset: 0x00112300
	Private Iterator Function shoot_left() As IEnumerator
		While True
			Me.basic.Create(Me.shoot2.transform.position, 0F, -Me.speed)
			Yield CupheadTime.WaitForSeconds(Me, 2F)
		End While
		Return
	End Function

	' Token: 0x040026C9 RID: 9929
	<SerializeField()>
	Private basic As BasicProjectile

	' Token: 0x040026CA RID: 9930
	<SerializeField()>
	Private shoot1 As Transform

	' Token: 0x040026CB RID: 9931
	<SerializeField()>
	Private shoot2 As Transform

	' Token: 0x040026CC RID: 9932
	<SerializeField()>
	Private shoot3 As Transform

	' Token: 0x040026CD RID: 9933
	Private speed As Single = 200F
End Class

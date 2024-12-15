from selenium import webdriver
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.chrome.service import Service as ChromeService
from webdriver_manager.chrome import ChromeDriverManager
import mysql.connector
import time
import json
import scan_loader
from datetime import datetime

def insert_result(loader,wap,ssid,channel,sig_str):
	try:

		connection = mysql.connector.connect(host='localhost',
											database='wireless_check',
											user='root',
											password='')
		cursor = connection.cursor()
		mysql_insert_query="""INSERT INTO result_radio (loader,mac_addr_wap,ssid,channel,signal_str) VALUES(%s,%s,%s,%s,%s)"""
		record = (loader,wap,ssid,channel,sig_str)
		cursor.execute(mysql_insert_query,record)
		connection.commit()
		print('sukses input')
	except mysql.connector.Error as error:
		print('failed insert'.format(error))

def vip(i):
	with webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()),options=options) as browser:
	#browser = webdriver.Chrome()
		browser.get('http://' + str(i))
		try:
			WebDriverWait(browser, 1).until(EC.presence_of_element_located((By.NAME, 'Login')))
			search=browser.find_element(By.NAME, 'Login')
			search.click()
			print('okeVIP')
		except TimeoutException:
			WebDriverWait(browser, 1).until(EC.presence_of_element_located((By.ID, 'unselect')))
			search=browser.find_element(By.ID, 'unselect')
			search.click()
			search=browser.find_element_by_xpath("//option[@value='Login']")
			search.click()
			print('okeACKSYS')
			time.sleep(10)

dt = datetime.now()
ts = datetime.timestamp(dt)

print(dt)
status = 0
online_loader = []
mac_addr=[]
ssid = []
channel = []
oru_loader=[]
signal=[]
scan_loader.check_online_loaders() #fungsi scanning loader return hasil yang online
options = webdriver.ChromeOptions()
count_of_loader = 0
print(count_of_loader) 
ssid_wap=[]
options.headless = True

print(scan_loader.online_loader)
print(status)
with webdriver.Chrome(service=ChromeService(ChromeDriverManager().install()),options=options) as browser:
	for i in scan_loader.online_loader:	
		try:
			
			browser.get('http://' +str(i))
			WebDriverWait(browser, 5).until(EC.presence_of_element_located((By.NAME, 'Login')))
			if browser.find_element(By.NAME, 'Login'):
				status=0
				login=browser.find_element(By.NAME,'Login')
				login.click()
				WebDriverWait(browser, 5).until(EC.presence_of_element_located((By.ID, 'associated_bssid')))
				browser.execute_script('window.open("http://10.10.10.38/associated_info.json")')
				time.sleep(3)
				ele_mac = browser.find_element(By.XPATH,'/html/body/form[1]/div/table/tbody/tr[3]/td/table/tbody/tr/td[2]/div/div[5]/table/tbody/tr[2]/td[2]/span')
				ele_ssid = browser.find_element(By.XPATH,'/html/body/form[1]/div/table/tbody/tr[3]/td/table/tbody/tr/td[2]/div/div[5]/table/tbody/tr[3]/td[2]/span')
				ele_channel = browser.find_element(By.XPATH,'/html/body/form[1]/div/table/tbody/tr[3]/td/table/tbody/tr/td[2]/div/div[5]/table/tbody/tr[4]/td[2]/span')
				ele_oru_loader = browser.find_element(By.XPATH,'/html/body/form[1]/div/table/tbody/tr[3]/td/table/tbody/tr/td[2]/div/div[2]/table/tbody/tr[4]/td[2]/span')
				ele_signal = browser.find_element(By.XPATH,'/html/body/form[1]/div/table/tbody/tr[3]/td/table/tbody/tr/td[2]/div/div[5]/table/tbody/tr[6]/td[2]/span')
				mac_addr.append(ele_mac.text)
				ssid.append(ele_ssid.text)
				channel.append(ele_channel.text)
				oru_loader.append(ele_oru_loader.text)
				signal.append(ele_signal.text+str('dBm'))

			
			else:
				print('gagal')
			

				#print(element)
				print('ada cuy'+str(i))
		except:
			print(str(i)+' bukan vip')
			status=1

		if status==1:
			try:
				
				browser.get('http://' +str(i))
				time.sleep(2)
				if browser.find_element(By.ID, 'unselect'):
					wireless_click=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[3]/table/tbody/tr/td/table/tbody/tr[3]/td/a')
					ele_oru_loader=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/div/fieldset[2]/fieldset/div[1]/div')
					oru_loader.append(ele_oru_loader.text)
					wireless_click.click()
					WebDriverWait(browser, 5).until(EC.presence_of_element_located((By.XPATH, '/html/body/div[2]/div/div[4]/form/div[2]/input[1]')))
					login=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/input[1]')
					login.click()
					browser.execute_script('window.open("")')
					time.sleep(3)
					ele_mac=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/fieldset/table/tbody/tr/td[5]')
					ele_ssid=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/fieldset/table/tbody/tr/td[3]')
					ele_channel=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/fieldset/table/tbody/tr/td[6]')
					ele_signal=browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/fieldset/table/tbody/tr/td[7]')
					mac_addr.append(ele_mac.text)
					ssid.append(ele_ssid.text)
					channel.append(ele_channel.text)
					signal.append(ele_signal.text)
					print('okaymantap')
					
				

			except:
				print('gaada')
		else:
			print('gagal'+str(i))
	print(oru_loader)
	print(mac_addr)
	print(ssid)
	print(channel)
	print(signal)
	print(mac_addr[1])
	co=0
connection = mysql.connector.connect(host='localhost',
										database='wireless_check',
										user='root',
										password='')
cursor = connection.cursor()
query_delete="""DELETE FROM result_radio WHERE channel >0"""
cursor.execute(query_delete)
connection.commit()

for i in oru_loader:
	lo=oru_loader[co]
	wap=mac_addr[co]
	wapm=wap[:-1]
	ss=ssid[co]
	ch=channel[co]
	sig=signal[co]
	print(lo,wapm,ss,ch,sig,dt)
	co+=1
	insert_result(lo,wapm,ss,ch,sig)
else:
	print('done slesai')

	
	
	#insert_result(lo,wapm,ss,ch,sig)



#for i in scan_loader.online_loader:
#	vip(i)

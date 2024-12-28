from msvcrt import getch
import string
from selenium import webdriver
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.by import By
from selenium.webdriver.support.ui import WebDriverWait
from selenium.webdriver.support import expected_conditions as EC
from selenium.common.exceptions import TimeoutException
from selenium.webdriver.remote.webelement import WebElement
from selenium.webdriver.chrome.service import Service as ChromeService
from webdriver_manager.chrome import ChromeDriverManager
from selenium.webdriver.support.select import Select
import sys
import sqlite3
import time

# print string(sys.argv[1]) + int(sys.argv[2])

def updatedetailORU(channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal):
    try:
        sqliteConnection = sqlite3.connect('C:/Users/TRAKINDO/source/repos/NCM/db/NM.db')
        cursor = sqliteConnection.cursor()
        print("Connected to SQLite")

        sqlite_update_with_param = """UPDATE tb_oru SET channel = ?, essid = ?, bridging = ?, 
		mac_cloning = ?, iscloning = ?, channelroam = ?, delay = ?, leave_threshold = ?, scan_threshold = ?, min_signal = ?, status = 'Online'
		WHERE no_loader = ?;"""
        data_tuple = (channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal, sys.argv[2])
        cursor.execute(sqlite_update_with_param, data_tuple)
        sqliteConnection.commit()
        print("Record Updated successfully")

        cursor.close()

    except sqlite3.Error as error:
        print("Failed to update data into sqlite table", error)
    finally:
        if sqliteConnection:
            sqliteConnection.close()
            print("The SQLite connection is closed")

# Variable for insert DB
channelconfig = string
essid = string
bridging = string
maccloning = string
iscloning = string
channelroam = string
delay = int
leavethreshold = int
scanthreshold = int
minsignal = int

options = webdriver.ChromeOptions()
options.headless = True

# Open ORU
print("Try open Chrome Webdriver")

def main():
	chrome_driver_path = 'C:/Users/TRAKINDO/Documents/Aby/chromedriver.exe'  # Replace with your path
	service = ChromeService(executable_path=chrome_driver_path)

	options = webdriver.ChromeOptions()
	options.headless = True

	with webdriver.Chrome(service=service, options=options) as browser:
		try:
			print("Try to open ORU " + sys.argv[1])
			browser.get('http://' + sys.argv[1])
			time.sleep(2)
			if sys.argv[1] != "":
				# Click Setup Tab
				setuptab = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[1]/ul/li[1]/a')
				setuptab.click()

				# Logic for Login
				# WebDriverWait(browser, 5).until(EC.presence_of_element_located((By.XPATH, '/html/body/div[2]/div/div[4]/form/div[2]/input[1]')))
				loginbtn = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/input[1]')
					
				loginbtn.click()
				browser.execute_script('window.open("")')
				time.sleep(3)
			
				try:
					if browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div/div'):
						insertpass = browser.find_element(By.NAME,'password')
						insertpass.send_keys("Minestar#1")
						loginbtn = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/input[1]')
						loginbtn.click()
						print("Click Login")
						browser.execute_script('window.open("")')
						time.sleep(3)
					else:
						loginbtn = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/input[1]')
						loginbtn.click()
						browser.execute_script('window.open("")')
						time.sleep(3)
						print("Login without password!!")
				except:
					print('Pass')
					pass
					
				# Select Wifi1 or Wifi2
				# getchannel = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div[1]/fieldset/table/tbody/tr[3]/td[2]')
				getchannel = browser.find_element(By.XPATH, "//div[1]/fieldset/table/tbody/tr[3]/td[2]")
				
				print(getchannel.text)

				if int(getchannel.text) == 6:
					wifi2btn = browser.find_element(By.XPATH,"//*[contains(@href, 'wireless_edit/radio1')]")
					wifi2btn.click()
					time.sleep(3)
					print("Open Wifi 2")
					
					# Get channel from Device Config and essid from Interface Config
					getchannelconfig = Select(browser.find_element(By.NAME,'cbid.wireless.radio1.channel_24'))
					getessid = browser.find_element(By.NAME,'cbid.wireless.radio1w0.ssid')
					
					# Go to Advanced Settings tab on Interface Config
					adsettab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[3]/a')
					adsettab.click()
					
					print("Open Tab")

					getbridging = Select(browser.find_element(By.ID,'cbid.wireless.radio1w0.bridge_mode'))
					
					if getbridging.first_selected_option.get_attribute('value') == '125nat':
						if browser.find_element(By.ID,'cbid.wireless.radio1w0.clone_mac'):
							iscloning = "0"
							getmaccloning = browser.find_element(By.ID,'cbid.wireless.radio1w0.clone_mac')
							maccloning = getmaccloning.get_attribute('value')
						else: 
							iscloning = "1"
							maccloning = ""
					else:
						iscloning = ""
						maccloning = ""
					
					# Go to Roaming tab
					roamtab  = browser.find_element(By.ID,'tab.wireless.radio1w0.roaming')
					roamtab.click()

					getchannelroam = Select(browser.find_element(By.ID,'cbid.wireless.radio1w0.scan_freq'))
					getdelay = browser.find_element(By.ID,'cbid.wireless.radio1w0.scan_interval')
					getleavethres = browser.find_element(By.ID,'cbid.wireless.radio1w0.leave_threshold')
					getscanthres = browser.find_element(By.ID,'cbid.wireless.radio1w0.scan_threshold')
					getminsignal = browser.find_element(By.ID,'cbid.wireless.radio1w0.roam_min_level')

					# Input data from element to variable
					channelconfig = getchannelconfig.first_selected_option.get_attribute('value')
					essid = getessid.get_attribute('value')
					bridging = getbridging.first_selected_option.get_attribute('value')
					channelroam = getchannelroam.first_selected_option.get_attribute('value')
					delay = getdelay.get_attribute('value')
					leavethreshold = getleavethres.get_attribute('value')
					scanthreshold = getscanthres.get_attribute('value')
					minsignal = getminsignal.get_attribute('value')
					
					print('Get Data ORU')
					
					# Insert data to DB
					updatedetailORU(channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal)
					browser.close()

				else:
					wifi1btn = browser.find_element(By.XPATH,"//*[contains(@href, 'wireless_edit/radio0')]")
					wifi1btn.click()
					time.sleep(3)
					print("Open Wifi 1")

					# Get channel from Device Config and essid from Interface Config
					getchannelconfig = Select(browser.find_element(By.NAME,'cbid.wireless.radio0.channel_24'))
					getessid = browser.find_element(By.NAME,'cbid.wireless.radio0w0.ssid')
					
					# Go to Advanced Settings tab on Interface Config
					adsettab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[3]/a')
					adsettab.click()
					
					print("Open Tab")

					getbridging = Select(browser.find_element(By.ID,'cbid.wireless.radio0w0.bridge_mode'))
					
					if getbridging.first_selected_option.get_attribute('value') == '125nat':
						if browser.find_element(By.ID,'cbid.wireless.radio0w0.clone_mac'):
							iscloning = "0"
							getmaccloning = browser.find_element(By.ID,'cbid.wireless.radio0w0.clone_mac')
							maccloning = getmaccloning.get_attribute('value')
						else: 
							iscloning = "1"
							maccloning = ""
					else:
						iscloning = ""
						maccloning = ""
					
					# Go to Roaming tab
					roamtab  = browser.find_element(By.ID,'tab.wireless.radio0w0.roaming')
					roamtab.click()

					getchannelroam = Select(browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_freq'))
					getdelay = browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_interval')
					getleavethres = browser.find_element(By.ID,'cbid.wireless.radio0w0.leave_threshold')
					getscanthres = browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_threshold')
					getminsignal = browser.find_element(By.ID,'cbid.wireless.radio0w0.roam_min_level')

					# Input data from element to variable
					channelconfig = getchannelconfig.first_selected_option.get_attribute('value')
					essid = getessid.get_attribute('value')
					bridging = getbridging.first_selected_option.get_attribute('value')
					channelroam = getchannelroam.first_selected_option.get_attribute('value')
					delay = getdelay.get_attribute('value')
					leavethreshold = getleavethres.get_attribute('value')
					scanthreshold = getscanthres.get_attribute('value')
					minsignal = getminsignal.get_attribute('value')
					
					print('Get Data ORU')
					
					# Insert data to DB
					updatedetailORU(channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal)
					browser.close()
		except:
			print('Data Unavailable')

if __name__ == "__main__":
    main()
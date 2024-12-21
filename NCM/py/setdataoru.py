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
# sys.argv[1] = ip loader, sys.argv[2] = no loader, sys.argv[3] = mac address,
# sys.argv[4] = channel, sys.argv[5] = essid, sys.argv[6] = bridging,
# sys.argv[7] = delay, sys.argv[8] = leave threshold, sys.argv[9] = scan threshold,
# sys.argv[10] = min signal

def updatedetailORU(channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal):
    try:
        sqliteConnection = sqlite3.connect('C:/Users/TRAKINDO/source/repos/NCM/db/NM.db')
        cursor = sqliteConnection.cursor()
        print("Connected to SQLite")

        sqlite_update_with_param = """UPDATE tb_oru SET channel = ?, essid = ?, bridging = ?, 
		mac_cloning = ?, iscloning = ?, channelroam = ?, delay = ?, leave_threshold = ?, scan_threshold = ?, min_signal = ? 
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
						insertpass.send_keys(sys.argv[5])
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
						print("Go go Login without password!!")
				except:
					print('Aneh nih')
					pass
					
				# Select Wifi1 or Wifi2
				# getchannel = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div[1]/fieldset/table/tbody/tr[3]/td[2]')
				getchannel = browser.find_element(By.XPATH, "//tr[3]/td[2]")
				
				print('Lewat Sini')

				if getchannel == 6:
					wifi2btn = browser.find_element(By.XPATH,"//*[contains(@href, 'wireless_edit/radio1')]")
					wifi2btn.click()
					time.sleep(3)
					print("Open Wifi 2")
				else:
					wifi1btn = browser.find_element(By.XPATH,"//*[contains(@href, 'wireless_edit/radio0')]")
					wifi1btn.click()
					time.sleep(3)
					print("Open Wifi 1")

				# Get channel from Device Config and essid from Interface Config
				setchannelconfig = Select(browser.find_element(By.NAME,'cbid.wireless.radio0.channel_24'))
				setchannelconfig.select_by_value(sys.argv[4])

				setessid = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[1]/div[3]/div/div/table/tbody/tr/td/input')
				setessid.send_keys(sys.argv[5])
				
				# Go to Advanced Settings tab on Interface Config
				adsettab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[3]/a')
				adsettab.click()
				
				print("udah buka advanced setting")

				setbridging = Select(browser.find_element(By.ID,'cbid.wireless.radio0w0.bridge_mode'))
				setbridging.select_by_value(sys.argv[6])
				
				if browser.find_element(By.ID,'cbid.wireless.radio0w0.clone_mac'):
					iscloning = "0"
					getmaccloning = browser.find_element(By.ID,'cbid.wireless.radio0w0.clone_mac')
					maccloning = getmaccloning.get_attribute('value')
				else: 
					iscloning = "1"
					maccloning = ""
				
				# Go to Roaming tab
				roamtab  = browser.find_element(By.ID,'tab.wireless.radio0w0.roaming')
				roamtab.click()

				setchannelroam = Select(browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_freq'))
				setchannelroam.select_by_value(sys.argv[11])
				setdelay = browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_interval')
				setdelay.send_keys(sys.argv[7])
				setleavethres = browser.find_element(By.ID,'cbid.wireless.radio0w0.leave_threshold')
				setleavethres.send_keys(sys.argv[8])
				setscanthres = browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_threshold')
				setscanthres.send_keys(sys.argv[9])
				setminsignal = browser.find_element(By.ID,'cbid.wireless.radio0w0.roam_min_level')
				setminsignal.send_keys(sys.argv[10])

				print('Capeeee')

				# saveconfig = browser.find_element(By.NAME,'cbi.apply')
				saveconfig = browser.find_element(By.CLASS_NAME,'cbi-button cbi-button-apply')
				saveconfig.click()
				print("Click Save")
				browser.execute_script('window.open("")')
				time.sleep(3)
				
				print('Set Data ORU')
				
				# Insert data to DB
				updatedetailORU(channelconfig, essid, bridging, maccloning, iscloning, channelroam, delay, leavethreshold, scanthreshold, minsignal)
				browser.close()
		except:
			print('Failed To Set Configuration')

if __name__ == "__main__":
    main()
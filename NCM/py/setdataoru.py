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
# sys.argv[4] = channel, sys.argv[5] = channelroam, sys.argv[6] = essid,
# sys.argv[7] = bridging, sys.argv[8] = delay, sys.argv[9] = leave threshold,
# sys.argv[10] = scan threshold, sys.argv[11] = min signal, sys.argv[12] = Bridging ID
# sys.argv[13] = Channel Roam ID

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
						print("Login without password")
				except:
					print('Pass')
					pass
					
				# Select Wifi1 or Wifi2
				# getchannel = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[1]/div[1]/fieldset/table/tbody/tr[3]/td[2]')
				getchannel = browser.find_element(By.XPATH, "//div[1]/fieldset/table/tbody/tr[3]/td[2]")
				
				print('Get Channel')

				if int(getchannel.text) == 6:
					wifi2btn = browser.find_element(By.XPATH,"//*[contains(@href, 'wireless_edit/radio1')]")
					wifi2btn.click()
					time.sleep(3)
					print("Open Wifi 2")

					# Get channel from Device Config and essid from Interface Config
					setchannelconfig = Select(browser.find_element(By.NAME,'cbid.wireless.radio1.channel_24'))
					setchannelconfig.deselect_all()
					setchannelconfig.select_by_value(sys.argv[4])

					setessid = browser.find_element(By.NAME,'cbid.wireless.radio1w0.ssid')
					setessid.clear()
					setessid.send_keys(sys.argv[6])
					
					# Go to Advanced Settings tab on Interface Config
					adsettab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[3]/a')
					adsettab.click()
					
					print("Open Tab")

					setbridging = Select(browser.find_element(By.ID,'cbid.wireless.radio1w0.bridge_mode'))
					setbridging.select_by_visible_text(sys.argv[7])

					if setbridging.first_selected_option.text != '125nat':
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

					setchannelroam = Select(browser.find_element(By.ID,'cbid.wireless.radio1w0.scan_freq'))
					setchannelroam.deselect_all()
					setchannelroam.select_by_value(sys.argv[5])
					setdelay = browser.find_element(By.ID,'cbid.wireless.radio1w0.scan_interval')
					setdelay.clear()
					setdelay.send_keys(sys.argv[8])
					setleavethres = browser.find_element(By.ID,'cbid.wireless.radio1w0.leave_threshold')
					setleavethres.clear()
					setleavethres.send_keys(sys.argv[9])
					setscanthres = browser.find_element(By.ID,'cbid.wireless.radio1w0.scan_threshold')
					setscanthres.clear()
					setscanthres.send_keys(sys.argv[10])
					setminsignal = browser.find_element(By.ID,'cbid.wireless.radio1w0.roam_min_level')
					setminsignal.clear()
					setminsignal.send_keys(sys.argv[11])

					saveconfig = browser.find_element(By.CLASS_NAME,'cbi-button-apply')
					saveconfig.click()

					print("Click Save")	

					print('Set Data ORU')
					
					# Insert data to DB
					updatedetailORU(sys.argv[4], sys.argv[6], sys.argv[7], sys.argv[3], iscloning, sys.argv[5], sys.argv[8], sys.argv[9], sys.argv[10], sys.argv[11])


					browser.close()
				else:
					wifi1btn = browser.find_element(By.XPATH,"//*[contains(@href, 'wireless_edit/radio0')]")
					wifi1btn.click()
					time.sleep(3)
					print("Open Wifi 1")

					print(sys.argv[4])
					print(sys.argv[5])
					# Get channel from Device Config and essid from Interface Config
					setchannelconfig = Select(browser.find_element(By.NAME,'cbid.wireless.radio0.channel_24'))
					setchannelconfig.deselect_all()
					setchannelconfig.select_by_value(sys.argv[4])

					setessid = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/div/div[1]/div[3]/div/div/table/tbody/tr/td/input')
					setessid.clear()
					setessid.send_keys(sys.argv[6])

					browser.save_screenshot(' tab general.png')
					
					# Go to Advanced Settings tab on Interface Config
					adsettab  = browser.find_element(By.XPATH,'/html/body/div[2]/div/div[4]/form/div[2]/fieldset[2]/ul/li[3]/a')
					adsettab.click()
					
					print("Open Tab")

					setbridging = Select(browser.find_element(By.ID,'cbid.wireless.radio0w0.bridge_mode'))
					setbridging.select_by_visible_text(sys.argv[7])

					print("Set Bridging Mode")

					if setbridging.first_selected_option.get_attribute('value') == '125nat':
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

					browser.save_screenshot('tab advanced.png')
					
					# Go to Roaming tab
					roamtab  = browser.find_element(By.ID,'tab.wireless.radio0w0.roaming')
					roamtab.click()

					setchannelroam = Select(browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_freq'))
					setchannelroam.deselect_all()
					setchannelroam.select_by_visible_text(sys.argv[5])
					setdelay = browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_interval')
					setdelay.clear()
					setdelay.send_keys(sys.argv[8])
					setleavethres = browser.find_element(By.ID,'cbid.wireless.radio0w0.leave_threshold')
					setleavethres.clear()
					setleavethres.send_keys(sys.argv[9])
					setscanthres = browser.find_element(By.ID,'cbid.wireless.radio0w0.scan_threshold')
					setscanthres.clear()
					setscanthres.send_keys(sys.argv[10])
					setminsignal = browser.find_element(By.ID,'cbid.wireless.radio0w0.roam_min_level')
					setminsignal.clear()  # Clear the existing value
					setminsignal.send_keys(sys.argv[11])  # Send the new value

					browser.save_screenshot('tab roam.png')

					saveconfig = browser.find_element(By.CLASS_NAME,'cbi-button-apply')
					saveconfig.click()

					print("Click Save")
					
					print('Set Data ORU')
					
					# Insert data to DB
					updatedetailORU(sys.argv[4], sys.argv[6], sys.argv[12], sys.argv[3], iscloning, sys.argv[13], sys.argv[8], sys.argv[9], sys.argv[10], sys.argv[11])
				
					browser.close()
		except:
			print('Failed To Set Configuration')

if __name__ == "__main__":
    main()